# 🚀 MyFirstDotnetApi

Simple .NET Web API project demonstrating REST API implementation with best practices.

## 📋 Prerequisites

- .NET 9.0 SDK
- Visual Studio Code with C# extension (optional)

## 🏁 Getting Started

1. Clone the repository
2. Navigate to the project directory
3. Run the project:
```bash
dotnet run
```

The API will start at `http://localhost:5012` by default ✨

For HTTPS support, run:
```bash
dotnet run --launch-profile https
```
This will start the API at `https://localhost:7177` and `http://localhost:5012` 🔒

## 👨‍💻 Development

- Build the project (compile only): `dotnet build`
- Run the project (build + start): `dotnet run`
- Watch mode (auto-rebuild on changes): `dotnet watch run`

## 📦 Dependencies

This project uses the following NuGet packages:
- `Microsoft.AspNetCore.Authentication.JwtBearer` (9.x) - JWT authentication middleware
- `Microsoft.AspNetCore.OpenApi` (9.x) - OpenAPI support for ASP.NET Core
- `System.IdentityModel.Tokens.Jwt` (8.x) - JWT token generation and validation

## 🏗️ Project Structure

- `/Controllers` - API endpoints (AuthController, UsersController)
- `/Models` - Data models (User, UserRequest, LoginRequest, RegisterRequest, RegisteredUser)
- `/Extensions` - Extension methods
- `/Helpers` - Helper classes
- `/Tests` - Empty directory for future test implementation

## 🔌 API Endpoints

- 🔐 Authentication: Login, Registration
- 👥 Users: CRUD operations

## 🔑 Authentication

This API uses JWT (JSON Web Tokens) for authentication:
- Tokens are issued upon successful login
- Protected endpoints require a valid JWT token in the Authorization header
- Example: `Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...`
- Tokens expire after a configured time period

## 🧪 Testing API

Use the provided `MyFirstDotnetApi.http` file to test API endpoints in VS Code with the REST Client extension.

## ⚙️ Configuration

- `appsettings.json` - Main configuration
- `appsettings.Development.json` - Development environment settings
- `Properties/launchSettings.json` - Application launch profiles 