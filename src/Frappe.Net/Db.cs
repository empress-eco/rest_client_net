﻿using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tiny.RestClient;

namespace Frappe.Net
{
    public class Db : JsonObjectParser
    {
        private const string RESOURCE_PATH = "/";
        private static readonly ILog log = LogManager.GetLogger(typeof(Db));
        Frappe frappe;
        TinyRestClient client;

        public Db(Frappe frappe)
        {
            this.frappe = frappe;
            this.client = frappe.Client;
        }

        /// <summary>
        /// Returns a list of records by filters, fields, ordering and limit
        /// 
        /// </summary>
        /// <param name="doctype">DocType of the data to be queried</param>
        /// <param name="fields"></param> fields to be returned. Default is `name`
        /// <param name="filters">filter list by this dict</param>
        /// <param name="orderBy">Order by this fieldname</param>
        /// <param name="limit_start">Start at this index</param>
        /// <param name="limitPageLength">Number of records to be returned (default 20)</param>
        /// <param name="parent"></param>
        /// <param name="debug"></param>
        /// <param name="asDict"></param>
        /// <returns></returns>
        public async Task<dynamic> GetListAsync(string doctype, string[] fields = null, string[,] filters = null, string orderBy= null, int limitStart= 0, int limitPageLength = 20, string parent = null, bool debug= false, bool asDict= true)
        {
            var request = client.GetRequest("frappe.client.get_list")
                .AddQueryParameter("doctype", doctype);

            if (fields != null)
                request.AddQueryParameter("fields", JsonConvert.SerializeObject(fields));
            
            if (filters != null)
                request.AddQueryParameter("filters", JsonConvert.SerializeObject(filters));

            if(orderBy != null)
                request.AddQueryParameter("order_by", orderBy);

            if (limitStart > 0)
                request.AddQueryParameter("limit_start", limitStart);

            if (limitPageLength != 20)
                request.AddQueryParameter("limit_page_length", limitPageLength);

            if (parent != null)
                request.AddQueryParameter("parent", parent);

            if(debug)
                request.AddQueryParameter("debug", true);

            if(!asDict)
                request.AddQueryParameter("as_dict", true);

            // TODO: Test all parameters

            string response;

            try {
                response = await request.ExecuteAsStringAsync();
            } 
            catch (Exception e)
            {
                throw;
            }
            return ToObject(response).message;
        }

        public async Task<int> GetCountAsync(string doctype, string[,] filters = null, bool debug = false, bool cache = false) {
            var request = client.GetRequest("frappe.client.get_count")
                .AddQueryParameter("doctype", doctype)
                .AddQueryParameter("debug", debug.ToString().ToLower());

            if (filters != null)
                request.AddQueryParameter("filters", JsonConvert.SerializeObject(filters));

            // HACK: Adding cache even when false return false result
            // TODO: Investigate
            if (cache)
                request.AddQueryParameter("cache", true);

            var response = await request.ExecuteAsStringAsync();
            var count = (JValue)ToObject(response).message;
            return count.ToObject<int>();
        }

        /// <summary>
        /// Returns a document by name or filters
        /// 
        /// </summary>
        /// <param name="doctype">DocType of the document to be returned</param>
        /// <param name="name">return document of this `name`</param>
        /// <param name="filters">If name is not set, filter by these values and return the first match</param>
        /// <param name="parent">If name is not set, filter by these values and return the first match</param>
        public async Task<dynamic> GetAsync(string doctype, string name = null, string[,] filters = null, string parent = null)
        {
            var request = client.GetRequest("frappe.client.get")
                .AddQueryParameter("doctype", doctype);

            if (name != null)
                request.AddQueryParameter("name", name);

            if (filters != null)
                request.AddQueryParameter("filters", JsonConvert.SerializeObject(filters));

            if (parent != null)
                request.AddQueryParameter("parent", parent);

            var response = "";
            try
            {
                response = await request.ExecuteAsStringAsync();
            }
            catch (HttpException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new KeyNotFoundException("The document with the supplied name does not exist");
                }
                throw;
            }

            return ToObject(response).message;
        }

        /// <summary>
        /// Returns a document by name or filters
        /// 
        /// </summary>
        /// <param name="doctype">DocType of the document to be returned</param>
        /// <param name="fieldName">Field to be returned (default `name`)</param>
        /// <param name="filters"></param>
        /// <param name="fieldName">return the document the `name` belongs</param>
        /// <param name="parent">If name is not set, filter by these values and return the first match</param>
        /// <returns></returns>
        public string GetValueAync(string doctype, string fieldName = null, string[,] filters = null, bool asDict = false, bool debug = false, string parent = null) {

            return "";
        }

        public async Task<dynamic> GetSingleValueAysnc(string doctype, string field)
        {
            var request = client.GetRequest("frappe.client.get_single_value")
                .AddQueryParameter("doctype", doctype)
                .AddQueryParameter("field", field);
            string response = "";

            try
            {
                response = await request.ExecuteAsStringAsync();
            }
            catch (Exception)
            {

                throw;
            }
            return ToObject(response).message;
        }

        public async Task<dynamic> SetValueAsync(string doctype, string name, string fieldName, string value = null)
        {
            var request = client.PostRequest("frappe.client.set_value")
                .AddQueryParameter("doctype", doctype)
                .AddQueryParameter("name", name)
                .AddQueryParameter("fieldname", fieldName)
                .AddQueryParameter("value", value);
            string response = "";

            try
            {
                response = await request.ExecuteAsStringAsync();
            }
            catch (Exception)
            {
                throw;
            }
            return ToObject(response).message;
        }

        /// <summary>
        /// Insert a document
        /// 
        /// </summary>
        /// <param name="doc">dictionary to be inserted</param>
        /// <returns>Dynamic object with properties of the documents that were saved.</returns>
        public async Task<dynamic> InsertAsync(Dictionary<string, object> doc) {
            var request = client.PostRequest("frappe.client.insert")
                .AddQueryParameter("doc", JsonConvert.SerializeObject(doc));
            string response = "";

            try
            {
                response = await request.ExecuteAsStringAsync();
            }
            catch (Exception)
            {

                throw;
            }
            return ToObject(response).message;
        }

        /// <summary>
        /// Insert multiple documents
        /// 
        /// </summary>
        /// <param name="docs">List containing dictionaries to be inserted</param>
        /// <returns>A list of IDs of the newly inserted documents</returns>
        public async Task<dynamic> InsertManyAsync(Dictionary<string, object>[] docs)
        {
            var request = client.PostRequest("frappe.client.insert_many")
                .AddQueryParameter("docs", JsonConvert.SerializeObject(docs));
            string response = "";

            try
            {
                response = await request.ExecuteAsStringAsync();
            }
            catch (Exception)
            {

                throw;
            }
            return ToObject(response).message;
        }

        /// <summary>
        /// Update (save) an existing document
        /// 
        /// </summary>
        /// <param name="doc">Dictionary object with the properties of the document to be updated</param>
        /// <returns>A dynamic object with the properties of the saved doc</returns>
        public async Task<dynamic> SaveAsync(dynamic doc)
        {
            // TODO: Find out why ```frappe.client.save``` does not work
            string doctype, name = "";
            if (doc.GetType() == typeof(Dictionary<String, object>))
            {
                doctype = ((Dictionary<String, object>)doc)["doctype"].ToString();
                name = ((Dictionary<String, object>)doc)["name"].ToString();
            }
            else { 
                doctype = doc.doctype;
                name = doc.name;
            }

            // HACK: Change API route from /api/method/frappe.client.save
            // to /api/resource/:doctype/:name format
            frappe.ChangeRoute($"/api/resource/{doctype}");
            IRequest request = frappe.Client.PutRequest(name)
                .AddStringContent(JsonConvert.SerializeObject(doc));
            string response = "";

            try
            {
                response = await request.ExecuteAsStringAsync();
            }
            catch (HttpException e)
            {
                log.Error($"{e.StatusCode} : {e.Message}");
                if (e.StatusCode == System.Net.HttpStatusCode.ExpectationFailed) {
                    throw new InvalidOperationException("Document not saved");
                }
            }
            catch (Exception e) {
                log.Error(e.Message);
            }
            frappe.ResetRoute();
            return ToObject(response).message;
        }

        /// <summary>
        /// Rename document
        /// </summary>
        /// 
        /// <param name="doctype">Doctype of the document to be renamed</param>
        /// <param name="oldName"Current `name` of the document to be renamed></param>
        /// <param name="newName">New `name` to be set</param>
        /// <param name="merge"></param>
        /// <returns>New name of the document after successful rename</returns>

        public async Task<string> renameDoc(string doctype, string oldName, string newName, int merge = 0) {
            // FIX: Find out reason empty response with actual rename

            var request = client.PostRequest("frappe.rename_doc")
                .AddQueryParameter("doctype", doctype)
                .AddQueryParameter("old", oldName)
                .AddQueryParameter("new", newName)
                .AddQueryParameter("merge", merge.ToString());
            string response = "";

            try
            {
                response = await request.ExecuteAsStringAsync();
            }
            catch (HttpException e)
            {
                log.Error($"{e.StatusCode} : {e.Message}");
                if (e.StatusCode == System.Net.HttpStatusCode.ExpectationFailed)
                {
                    throw new InvalidOperationException("Unable to rename document");
                }
            }
            return ToObject(response).message.ToObject<string>();
        }

        /// <summary>
        /// Submit a document
        /// </summary>
        /// <param name="doc">JSON or dict object to be submitted remotely</param>
        /// <returns>The submitted document</returns>
        public async Task<dynamic> SubmitAsync(Dictionary<string, object> doc)
        {
            var request = client.PostRequest("frappe.client.submit")
                .AddQueryParameter("doc", JsonConvert.SerializeObject(doc));
            string response = "";

            try
            {
                response = await request.ExecuteAsStringAsync();
            }
            catch (Exception)
            {

                throw;
            }
            return ToObject(response).message;
        }

        /// <summary>
        /// Cancel a document
        /// </summary>
        /// <param name="doctype">Doctype of the document to be cancelled</param>
        /// <param name="name">Name of the document</param>
        /// <returns>The document that was cencelled</returns>
        public async Task<dynamic> CancelAsync(string doctype, string name)
        {
            var request = client.PostRequest("frappe.client.cancel")
                .AddQueryParameter("doctype", doctype)
                .AddQueryParameter("name", name);
            string response = "";

            try
            {
                response = await request.ExecuteAsStringAsync();
            }
            catch (Exception)
            {

                throw;
            }
            return ToObject(response).message;
        }

        /// <summary>
        /// Delete a remote document
        /// </summary>
        /// <param name="doctype">DocType of the document to be deleted</param>
        /// <param name="name">Name of the document to be deleted</param>
        /// <returns></returns>
        public async Task<dynamic> DeleteAsync(string doctype, string name)
        {
            var request = client.PostRequest("frappe.client.delete")
                .AddQueryParameter("doctype", doctype)
                .AddQueryParameter("name", name);
            string response = "";

            try
            {
                response = await request.ExecuteAsStringAsync();
            }
            catch (HttpException e)
            {
                if(e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new KeyNotFoundException("The document with the supplied name does not exist");
                }
                throw;
            }
            return ToObject(response).message;
        }
    }
}
