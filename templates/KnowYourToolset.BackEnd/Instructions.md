# KnowYourToolset.BackEnd - Overview and Instructions

This solution was generated from an ASP.NET Core WebAPI template that comes with a bundle of features:
- Semantic Versioning for the API via the URL Path
- OpenAPI (Swagger) document generation and user interface
- Error Logging to Seq and Console
- Liveness health check at `/health`
- Global error handling and shielding using ProblemDetails
- Request logging for HTTP status and elapsed time of requests and logged-in user
- Unit tests for business logic

## In The Box

### Api
The main API project. It uses the following NuGet packages:
- [Swashbuckle.AspNetCore](https://github.com/domaindrivendev/Swashbuckle.AspNetCore) to provide Swagger documents.
- [Swashbuckle.AspNetCore.Annotations](https://github.com/domaindrivendev/Swashbuckle.AspNetCore.Annotations) to provide additional information into the Swagger documents.
- [API Versioning](https://github.com/microsoft/aspnet-api-versioning/wiki/Versioning-via-the-URL-Path) Versioning with semantics via the URL Path
- [Serilog.AspNetCore](https://tfs.realpage.com/tfs/Realpage/StarterKits/_git/realpage-logging-serilog?_a=readme) for the logging.

This API includes error handling by default that will log exceptions with full details but provide an error response with an ID to the caller.

## Solution Structure 
All of the "concerns" below are contained in their own folders.  

### API 
This is the project that contains the API being developed.

### Business Logic
This code represents the real logic of the application and some scaffolded code is included here as an example.  You would replace this code with your own logic and classes, but you can follow the examples if you need to understand how all of the pieces fit together.

### Tests/LogicTests
This test project is intended to be used to test as much of the **business logic** in the solution as possible.  It uses the following nuget packages:
- [XUnit](https://xunit.net/) as the test framework
- [Moq](https://github.com/moq/moq4) as the mocking framework
- [FluentAssertions](https://fluentassertions.com/) to make the assert statements more readable.

There are some working unit tests written already against the sample business logic.  As you write your own business logic -- or maybe even **before** you write it, you should write unit tests following similar patterns.

## Configuration
The primary settings for the API are defined in the `appSettings.json` file -- and there aren't different versions of this file to support different environments.  Different values for other environments are provided in release definitions (see **Continuous Delivery** below).

Name | Purpose | Sample value
--- | --- | ---
`Authentication.Authority` | Defines the OAuth2 authority that is trusted by the API | https://demo.duendesoftware.com/
`Authentication.SwaggerClientId` | OpenID Connect client id for the Swagger UI | `implicit.public`
`Authentication.ApiName` | OAuth2 scope(s) that will authorize the caller for this api.  Any scope(s) (space-separated) specified here will be required.  | `api`
`Authentication.AdditionalScopes` | Optional OAuth2 scope(s) that can be used in authorization policies within this api.  Any scope(s) (space-separated) specified here will ***not*** be required.  | `openid profile email`
`Serilog` Settings | [Settings for Serilog](https://github.com/serilog/serilog-settings-configuration) that control the level of log entries that are actually logged | JSON to indicate default min levels for various source contexts


## Running the Solution
Running the application will open a web browser to the Swagger user interface.  Test away!  See below for some things to try.  :)

## Continuous Integration and Delivery
Forthcoming....

# Things to Try
* Open the solution and explore 
* Run the solution - Swagger UI page should be opened
  - Verify that just trying a method gives you a `401` response (the API requires authentication)
  - Click `Authorize` and sign in via a demo instance of the Duende IdentityServer (credentials are on the login page)
  - Now try the main `WeatherForecast` method - it should work
  - Use a value of `11111` for the `postalCode` input and it will throw an exception and return a `500` response
  - Use a value of `22222` for the `postalCode` input and it will throw an exception and return a `400` response with an additional message
* Check log entries
  - Should be available at http://localhost:5341
* Run unit tests
  - A few unit tests are included with mock data
* Check out the liveness health check at `/health`
