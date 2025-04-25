# KindaMovies API

A RESTful API built with Azure Functions that provides access to a database of movies filmed in San Francisco. The API allows users to retrieve movie information with various filtering and search capabilities.

## Features

- Get all movies in the database
- Get paginated movie results
- Search movies by title term
- RESTful endpoints following API versioning best practices
- Clean architecture with separation of concerns
- Entity Framework Core for data access
- Azure Functions for serverless deployment

## Architecture

The project follows a clean architecture pattern with the following layers:

- **Domain**: Contains business entities, interfaces, and models
- **Application**: Contains business logic and services
- **Infrastructure**: Contains data access and repository implementations
- **Functions**: Contains Azure Functions endpoints

## API Endpoints

### Get All Movies
```
GET /v1/movie/all
```
Returns all movies in the database.

### Get Paginated Movies
```
GET /v1/movie/pag?pageNumber=1&pageSize=10
```
Returns a paginated list of movies.

Query Parameters:
- `pageNumber`: The page number to retrieve (default: 1)
- `pageSize`: Number of items per page (default: 10)

### Search Movies
```
GET /v1/movie/search?term=matrix
```
Searches for movies containing the specified term in their title.

Query Parameters:
- `term`: The search term to look for in movie titles

## Response Format

All endpoints return JSON responses with the following structure:

### Get All Movies
```json
{
    "Movies": [
        {
            "Id": 1,
            "Title": "Movie Title",
            "ReleaseYear": 1999,
            "Locations": "San Francisco",
            "FunFacts": "Interesting fact",
            "ProductionCompany": "Company Name",
            "Distributor": "Distributor Name",
            "Director": "Director Name",
            "Writer": "Writer Name",
            "Actor1": "Actor 1",
            "Actor2": "Actor 2",
            "Actor3": "Actor 3"
        }
    ]
}
```

### Get Paginated Movies
```json
{
    "TotalCount": 100,
    "PageNumber": 1,
    "PageSize": 10,
    "Movies": [
        // Array of movie objects
    ]
}
```

### Search Movies
```json
{
    "SearchTerm": "matrix",
    "Movies": [
        // Array of matching movie objects
    ]
}
```

## Error Responses

In case of errors, the API returns a 400 Bad Request with the following format:

```json
{
    "error": "Error message describing what went wrong"
}
```

## Prerequisites

- .NET 8.0 SDK
- Azure Functions Core Tools
- SQL Server (local or Azure)
- Azure subscription (for deployment)

## Local Development

1. Clone the repository
2. Update the connection string in `local.settings.json`
3. Run the following commands:
   ```bash
   dotnet restore
   dotnet build
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   func start
   ```

## Configuration

The application requires the following configuration:

- `SqlDb`: SQL Server connection string
- `AzureWebJobsStorage`: Azure Storage connection string (for local development, use "UseDevelopmentStorage=true")

