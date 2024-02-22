<p align="center">
  <img src="https://empress.eco/images/logo.png" alt="Empress Logo">
</p>

<p align="center">
Empower your .NET development with a robust REST client designed to simplify your interaction with online resources.
</p>

<p align="center">
<a href="https://empress.eco/">Official Website</a> · <a href="https://github.com/empress-eco/rest_client_net/issues">Report Bug</a> · <a href="https://github.com/empress-eco/rest_client_net/issues">Request Feature</a>
</p>

## 🚀 About The Project

Empress REST client for .NET is a comprehensive library that simplifies the process of making RESTful requests, managing authentication, and handling database operations. It's an indispensable tool for developers seeking to streamline their .NET development workflow.

### 💎 Key Features

- **Versatile Authentication**: Supports Token Based, Password Based, and Access Token methods.
- **Debugging Mode**: HTTP Request Logging for efficient troubleshooting.
- **Comprehensive Database Functions**: Includes functions for Listing Documents, Getting Count, Getting Single Document, and much more.
- **Fluent Style Syntax**: Ensures easy and intuitive use.

## 🛠️ Technical Stack and Setup Instructions

### Prerequisites

- .NET installed on your system.

### Installation

Clone the repository to your local machine:

```sh
git clone https://github.com/empress-eco/rest_client_net.git
```

Install the package via NuGet Package Manager:

```sh
Install-Package EmpressRestClient.Net
```

## 📖 Usage

Create a client, authenticate, and you're ready to make requests:

```cs
using Empress.Net;

var Empress = new Empress("https://base-url.com/");
await Empress.UseTokenAync("api-key", "api-secret");
```

For further details, refer to our [Official Documentation](https://grow.empress.eco/).

## 🤝 Contribution Guidelines

We welcome and appreciate contributions from the community! Here's how you can contribute:

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📜 License and Acknowledgements

The project is under the MIT License. Your contributions are also licensed under the MIT License. 

We express our deep gratitude to the Empress Community, the creators of the essential tools that power this project. Their innovation and dedicated work have been crucial in building the foundations and functionalities we rely on. We are profoundly grateful for their pioneering work and ongoing support.
