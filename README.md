# SensitiveWordsAPI

A C# .NET 8 microservice that detects and censors sensitive words. Built with Clean Architecture, Dapper, and Swagger.

## Features
- REST API with Swagger docs
- API Key authentication
- Word CRUD endpoints (internal)
- Sanitization endpoint (external)
- Exception handling middleware
- xUnit + Moq unit tests

## Getting Started

1. Clone the repo
2. Run `dotnet restore`
3. Update `appsettings.json` with your API key
4. Run the project: `dotnet run`

API Docs available at `/swagger`.

## Author

Wouter Human – wouterhuman@gmail.com
