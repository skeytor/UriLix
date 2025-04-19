# UriLix Project

## Overview
UriLix is a .NET 9-based project designed to provide robust and scalable URL Shortener Web API. The project includes multiple components, such as APIs and application logic, with a focus on modularity and testability.

## Key Features
- Built on .NET 9 for modern performance and features.
- Authentication and Authorization.
- Redis caching.
- Modular architecture with separate API and application layers.
- Comprehensive unit and integration tests using xUnit.
- Docker support
- Integration with Testcontainers for integration testing.

## Getting Started

### Prerequisites
- Docker & Docker Compose

### Installation
1. Clone the repository
```bash
https://github.com/skeytor/UriLix.git
cd UriLix
```
2. Set up the app

Initialize the user secrets
```bash
dotnet user-secrets init --project ./src/UriLix.API
```

Sets the secret values
```bash
touch secrets.json
notepad secrets.json
```
Copy the template below, replace the placeholder values with your actual secrets, and save it as `secrets.json`:

```json
{
  "Jwt:SecretKey": "your-secret-key-123-32-bits",
  "Jwt:Issuer": "https://localhost:5001",
  "Jwt:Audience": "https://localhost:5001",
  "ConnectionStrings:Redis": "urilix.redis:6379,password=YourStrong#Pass123,abortConnect=false,connectTimeout=30000,connectRetry=5",
  "ConnectionStrings:Database": "Server=urilix.database;Database=UriLix;User ID=sa;Password=YourStrong#Pass123;TrustServerCertificate=True",
  "Authentication:GitHub:UserInformationEndpoint": "https://api.github.com/user",
  "Authentication:GitHub:TokenEndpoint": "https://github.com/login/oauth/access_token",
  "Authentication:GitHub:ClientSecret": "your_client_secret_23",
  "Authentication:GitHub:ClientId": "your_client_id_32",
  "Authentication:GitHub:CallbackPath": "/your/github/callback",
  "Authentication:GitHub:AuthorizationEndpoint": "https://github.com/login/oauth/authorize"
}
```
Then imports the secrets file
```bash
cat secrets.json | dotnet user-secrets set --project ./src/UriLix.API
rm secrets.json
```

3. Run unit & integration tests
```bash
dotnet test UriLix.sln
```

4. Run and launch the app.
```bash
docker-compose up -d --build
```

5. Access to Scalar Web Api Doc by [Scalar Docs](https://localhost:5001/scalar/v1)
