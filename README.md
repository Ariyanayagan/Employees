# 🏗️ Clean Architecture ASP.NET Core MVC Project

Welcome to the Clean Architecture ASP.NET Core MVC Project! This project is designed with a focus on maintainability, scalability, and testability, using a Clean Architecture approach. The application includes social authentication using Google and integrates with various tools like MediatR, Microsoft Identity, AutoMapper, and Google API.

## 🌟 Features

- **Clean Architecture**: Follows the principles of Clean Architecture to ensure a scalable and maintainable codebase.
- **MediatR**: Facilitates decoupled communication between components with CQRS (Command Query Responsibility Segregation).
- **Microsoft Identity**: Provides robust user authentication and authorization.
- **Social Authentication**: Enables users to sign in with their Google accounts.
- **AutoMapper**: Simplifies object mapping in the application.
- **Google API Integration**: Utilizes Google services for various functionalities.

<br/>

## 📂 Project Structure

The project is organized into the following layers:

| Layer                     | Description                                                                 |
| ------------------------- | --------------------------------------------------------------------------- |
| **Presentation**          | Contains the ASP.NET Core MVC controllers and views.                        |
| **Application**           | Holds the business logic, including MediatR commands and queries.           |
| **Domain**                | Contains the core entities and business rules.                              |
| **Infrastructure**        | Manages data access, identity, and external services like Google API.       |

<br/>

## 📦 Dependencies

| Package                                                 | Description                                                     |
| ------------------------------------------------------- | --------------------------------------------------------------- |
| `MediatR.Extensions.Microsoft.DependencyInjection`      | For CQRS pattern implementation using MediatR.                  |
| `Microsoft.AspNetCore.Identity.EntityFrameworkCore`     | For managing identity with Entity Framework Core.               |
| `AutoMapper.Extensions.Microsoft.DependencyInjection`   | For automatic object-object mapping.                            |
| `Google.Apis.Auth`                                      | For Google authentication integration.                          |

<br/>

## 🚀 Contributing

Contributions are welcome! Please create a fork of the repository and submit a pull request with your changes.

## 📄 License

This project is licensed under the MIT License. See the [LICENSE](LICENSE.txt) file for more details.

<br/>

### Key Sections Breakdown:

- **Features**: Highlight key aspects of your project.
- **Project Structure**: Provides a table to describe each layer in your Clean Architecture.
- **Contributing**: Encourages collaboration.
- **Dependencies**: List out all Dependencies.
- **License**: Mentions the license.

This `README.md` should give users a clear understanding of your project and how to get started.

