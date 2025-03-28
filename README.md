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

## 📚 API Documentation (Swagger)

The API is fully documented using Swagger/OpenAPI:

1. Start the application with `dotnet run`
2. Open your browser and navigate to [http://localhost:5012](http://localhost:5012)
3. The Swagger UI will load automatically, showing all available endpoints

### Using Swagger UI

1. **Browse API endpoints** - Organized into Authentication and Users categories
2. **Try out endpoints** - Test API calls directly from the UI
3. **Authentication** - Use the following steps to test protected endpoints:
   - First, execute the `/auth/register` endpoint to create a user
   - Then, call the `/auth/login` endpoint to get a JWT token
   - Click the "Authorize" button at the top of the page
   - Enter the token with format: `Bearer your-token-here`
   - Now you can access protected endpoints

The documentation includes:
- Request/response models with examples
- Parameter descriptions and validation requirements
- Authentication requirements
- Response status codes and descriptions

## 👨‍💻 Development

- Build the project (compile only): `dotnet build`
- Run the project (build + start): `dotnet run`
- Watch mode (auto-rebuild on changes): `dotnet watch run`

## 📦 Dependencies

This project uses the following NuGet packages:
- `Microsoft.AspNetCore.Authentication.JwtBearer` (9.x) - JWT authentication middleware
- `Microsoft.AspNetCore.OpenApi` (9.x) - OpenAPI support for ASP.NET Core
- `Swashbuckle.AspNetCore` (6.5.0) - Swagger documentation generation
- `System.IdentityModel.Tokens.Jwt` (8.x) - JWT token generation and validation

## 🏗️ Project Structure

- `/Controllers` - API endpoints (AuthController, UsersController)
- `/Models` - Data models (User, UserRequest, LoginRequest, RegisterRequest, RegisteredUser)
- `/Extensions` - Extension methods
- `/Helpers` - Helper classes
- `/Tests` - Empty directory for future test implementation

## 🔌 API Endpoints

### Authentication Endpoints
- `POST /auth/register` - Register new user
  - Request: User details (FirstName, LastName, Email, PhoneNumber, Password)
  - Response: User ID and confirmation message
- `POST /auth/login` - Authenticate user
  - Request: Email and Password
  - Response: JWT token with expiration info

### User Management Endpoints
- `GET /users` - List all users (requires auth)
- `POST /users` - Create user (no auth required)
- `GET /users/{id}` - Get specific user (requires auth)
- `PUT /users/{id}` - Update user (requires auth)
- `DELETE /users/{id}` - Delete user (requires auth)
- `GET /users/me` - Get current user profile (requires auth)

### Misc
- `GET /ping` - Health check (no auth)

## 🔑 Authentication

This API uses JWT (JSON Web Tokens) for authentication:
- Tokens are issued upon successful login
- Protected endpoints require a valid JWT token in the Authorization header
- Example: `Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...`
- Tokens expire after a configured time period

## 🧪 Testing API

Use the provided `MyFirstDotnetApi.http` file to test API endpoints in VS Code with the REST Client extension.

Alternatively, use the built-in Swagger UI at [http://localhost:5012](http://localhost:5012) for interactive testing.

## ⚙️ Configuration

- `appsettings.json` - Main configuration including JWT settings
- `appsettings.Development.json` - Development environment settings
- `Properties/launchSettings.json` - Application launch profiles 

## 💾 Data Storage

This application uses in-memory storage for data persistence:
- All data is stored in memory using `List<T>` collections
- Data is registered as singletons in the dependency injection container
- User data is stored in a `List<User>` collection
- Authentication data is stored in a `List<RegisteredUser>` collection
- All data is lost when the application restarts

This is a simple demonstration API and not intended for production use. In a real-world scenario, you would implement:
- A database (SQL Server, PostgreSQL, MongoDB, etc.)
- Entity Framework Core or another ORM
- Proper data persistence and backup mechanisms 