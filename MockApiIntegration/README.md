# Mock API Integration

A .NET 8.0 Web API that integrates with the mock API at [https://restful-api.dev](https://restful-api.dev/).

## Features

- Retrieve products with name filtering and paging
- Create new products
- Delete existing products
- Global error handling
- Input validation

## Setup

1. Make sure you have .NET 8.0 SDK installed
2. Clone the repository
3. Navigate to the project directory
4. Run `dotnet restore`
5. Run `dotnet run`

The API will be available at:

- http://localhost:5082

Swagger UI will be available at:

- http://localhost:5082/swagger

## API Endpoints

### Get Products

```
GET /api/products?name={nameFilter}&page={pageNumber}&pageSize={pageSize}
```

- `name` (optional): Filter products by name
- `page` (optional): Page number (default: 1)
- `pageSize` (optional): Items per page (default: 10)

### Create Product

```
POST /api/products
```

Request body:

```json
{
  "name": "Product Name",
  "price": 99.99,
  "description": "Product description"
}
```

### Delete Product

```
DELETE /api/products/{id}
```

## Error Handling

The API includes global error handling that returns errors in the following format:

```json
{
  "statusCode": 500,
  "message": "An internal server error occurred.",
  "detailedMessage": "Error details..."
}
```

## Validation

The API implements model validation:

- Name is required and limited to 100 characters
- Price is required
- Description is optional but limited to 500 characters
