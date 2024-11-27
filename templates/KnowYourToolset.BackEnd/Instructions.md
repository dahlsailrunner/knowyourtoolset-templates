# KnowYourToolset.BackEnd - Overview and Instructions

This solution was generated from an ASP.NET Core WebAPI template that comes with a bundle of features:

- Semantic Versioning for the API via the URL Path
- CRUD operations with FluentValidation for `Product` entities
- Paginated results on the `v2` version of the `GET products` method
- OpenAPI (Swagger) document generation and user interface
- OAuth2 authentication with [Duende IdentityServer demo instance](https://demo.duendesoftware.com/) (can be updated to any OAuth2 provider)
- Global error handling and shielded errors with `ProblemDetails`
- Liveness health check at `/health`
- Open Telemetry tracing, logging, and metrics
- Unit tests for business logic
- EF Core data access with open telemetry tracing via Aspire (PostgreSQL and SQL Server)

## Things to Try

- Open the solution and explore 
- Run the solution - Swagger UI page should be opened
  - Verify that just trying a method gives you a `401` response (the API requires authentication)
  - Click `Authorize` (select all scopes and leave the `client secret` blank) and sign in via a demo instance of the Duende IdentityServer (credentials are on the login page)
  - Now try the main `WeatherForecast` method - it should work
  - Use a value of `11111` for the `postalCode` input and it will throw an exception and return a `500` response
  - Use a value of `22222` for the `postalCode` input and it will throw an exception and return a `400` response with an additional message
- Run unit tests
  - A few unit tests are included with mock data
- Check out the liveness health check at `/health`
