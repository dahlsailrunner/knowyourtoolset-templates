# KnowYourToolset.BackEnd - Overview and Instructions

> This solution is **immediately runnable**! So just run it!  What follows are notes explaining
> some of the different features you may be interested in.

This solution was generated from an ASP.NET Core WebAPI template that comes with a bundle of features:

- [OAuth2 (JWT Bearer) authentication](#authentication)
- [EF Core data access](#data) with open telemetry tracing via Aspire (PostgreSQL and SQL Server)
- [OpenAPI (Swagger)](#openapi) document generation and user interface
- [Global error handling](#error-handling) and shielded errors with `ProblemDetails`
- [Open Telemetry](#open-telemetry) tracing, logging, and metrics
<!--#if ANGULAR-->
- [Angular Material user interface](#angular-ui) using a BFF for improved security
<!--#endif-->
- [CRUD operations with FluentValidation](#validation) for `Product` entities
- [Semantic Versioning](#versioning) for the API via the URL Path
- Paginated results on the `v2` version of the `GET products` method
- Liveness health check at `/health`
- Unit tests for business logic

## Authentication

No anonumous access is allowed on any API methods. An OAuth2 bearer token is
required to authenticate, and is provided by the [Demo Duende IdentityServer](https://demo.duendesoftware.com).
The primary configuration is in the `Authentication` section of `appsettings.json`.

The logged-in user is included in log entries via the
`UserScopeMiddleware` - see [Open Telemetry](#open-telemetry).

Changing to use another provider like [Auth0](https://auth0.com) or anything else should be mostly just changing
those cofig values.

Note that the ONLY reason there is a `swaggerClientId` is for the Swagger UI, which will only
be present in `Development` as coded (see the [OpenAPI](#openapi) section below for more details)

## Data

An EF Core database is included with this project as a sample.  It has a single table
called `Products`.

Sample data is generated in `Development` during startup and uses the [Bogus](https://github.com/bchavez/Bogus)
library.

SQL Server and PostgreSQL will both include Open Telemetry information when used (logs, metrics,
traces) and use Aspire hosting with the containers for those databases.  SQLite uses a file-based
database and does not include traces.

### Migrations

There is some code in `Program.cs` of the API:

```csharp
if (args.Contains("--migrateDb")) // use this as a "job" to apply migrations in non-dev
{
    app.ApplyMigrations();
    return 0;
}
if (app.Environment.IsDevelopment()) // do migrations / set up data differently in non-dev
{
    app.SetupDevelopmentDatabase();
}
```

You can make this code whatever you want (or completely remove it).  In `Development`, it
always applies migrations and sets up some initial sample data from scratch as it's
starting up the API.

For a higher-environment where you might be running more than one instance of the API,
you might want to use a "job-based" approach, where any new migrations can be applied
before starting ANY of the API instances. If the command line arg of `--migrateDB` is
passed, then the API will simply apply the migrations and exit.  This could be a way
to run migrations before starting any of the API instances.  Use whatever
approach for migrations works for you.

## OpenAPI

A Swagger user interface can be helpful in local testing, and is especially helpful
for this API which requires authentication with bearer tokens.

For that reason, the [Swashbuckle](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)
NuGet package has been included and used (the [Scalar UI project](https://github.com/scalar/scalar)
doesn't currently support interactive authentication to get a bearer token).

In general, don't include the Swagger UI in production environments (or maybe even anything
non-development, like this project does).

### Authorizing for API Calls

To make an authenticated API call using the Swagger UI:

- Click `Authorize` in the Swagger UI
- Leave the `client_id` as interactive.public, leave the `client_secret` blank and select all scopes
- Click `Authorize` (you may need to scroll down to see the button)
- Log in (instructions on the login page)
- Click `Close`

## Error Handling

Any errors in the API will return a[ProblemDetails](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.problemdetails)
response.

The response handling is slightly customized via the `AddApiProblemDetails` method, defined in the
`ServiceDefaults` project.  It looks for certain exceptions and will return `400 Bad Request`
responses for things like validation failures.  Any general error will be shielded from the
caller but fully logged.  The response to the caller will include the Open Telemetry trace
id, which can be used for looking up the error in the logs.

## Open Telemetry

The Aspire opinions are used almost with the defaults for Open Telemetry - which includes
logs, traces, and metrics.

Since authentication is a first-class concern in this project, the `UserScopeMiddleware` has
been defined in the `ServiceDefaults` project to include information about the logged-in
user in any log entries or exception details.

### Custom Traces

Activities like data access, HTTP requests, Redis calls, and more can be captured without
any custom instrumentation - just use the Aspire libraries.  But sometimes there is logic
in our code that we'd like to capture as a trace.

An `ActivitySource` for the ASP.NET projects that use the `AddServiceDefaults()` method
is available to be injected wherever you need it.  Then to create a trace, simply add a line
that looks something like the following:

```csharp
using var activity = _activitySource.StartActivity($"VALIDATION {GetType().Name}");
```

The upper-case first word follows the convention of other traces already being
captured and should indicate the type of activity being performed (other examples are
`DATA` for database activity and HTTP verbs like `GET` or `POST` for http requests).

To add custom information into the activity, you can use the `SetTag` method:

```csharp
activity?.SetTag("product", context.InstanceToValidate.Name);
```

See the `ProductValidator` for an example of this - it provides an override for the
`ValidateAsync` method that includes a trace for the validation activity.  Feel
free to remove this if you don't need it.

### An External Open Telemetry Service

All of the Open Telemetry information goes to the Aspire Dashboard for local development.

To use an Open Telemetry or Application Performance Monitoring (APM) service, though, is easy.

Mostly this would involve setting two environment variables to values for your service:

- `OTEL_EXPORTER_OTLP_ENDPOINT`: the URL for your APM service
- `OTEL_EXPORTER_OTLP_HEADERS`: an authentication token for your service (keep this secret)

Here are some links for more information about Open Telemetry configuration:

- [General envrionment variables reference for .NET OpenTelemetry](https://github.com/open-telemetry/opentelemetry-dotnet/blob/main/src/OpenTelemetry.Exporter.OpenTelemetryProtocol/README.md#environment-variables)
- [Details about the `OTEL_RESOURCE_ATTRIBUTES` options](https://opentelemetry.io/docs/specs/semconv/resource/)
- [How sampling works](https://opentelemetry.io/docs/specs/otel/trace/sdk/#sampling)
- [How to set sampling-related configuration](https://github.com/open-telemetry/opentelemetry-specification/blob/main/specification/configuration/sdk-environment-variables.md)

> **T I P:** To see a list of Open Telemetry configuration set by the Aspire App Host,
> use a breakpoint somewhere in your project code and look at the `OTEL_*` values in the
> `Configuration` instance for your app.

#### Popular Services for Open Telemetry

Many of the services below will allow you to start for free.  Make sure to understand
pricing at the level of scale you require - this can be data storage,
number of users that need access to the platform, number of transactions sent,
or other factors.

- [Elastic APM](https://www.elastic.co/observability/application-performance-monitoring): Elastic integrates with OpenTelemetry, allowing you to reuse your existing instrumentation to send observability data to the Elastic Stack.
- [Datadog](https://www.datadoghq.com/): Datadog supports OpenTelemetry for collecting and visualizing traces, metrics, and logs.
- [New Relic](https://newrelic.com/): New Relic offers support for OpenTelemetry, enabling you to send telemetry data to their platform.
- [Splunk](https://www.splunk.com/): Splunk's Observability Cloud supports OpenTelemetry for comprehensive monitoring and analysis.
- [Honeycomb](https://www.honeycomb.io/): Honeycomb integrates with OpenTelemetry to provide detailed tracing and observability.

#### Testing an Open Telemetry Service Locally

A simple extension method called `WithOtherOpenTelemetryService` has been
added to the AppHost code in `LoggingHelper.cs`.

Calls to this method can be added in
`Program.cs` of the AppHost to send local Open Telemetry
data to a different place than the Aspire Dashboard.

```csharp
var api = builder.AddProject<Projects.KnowYourToolset_BackEnd_Api>("knowyourtoolset-backend-api")
    .WithOtherOpenTelemetryService(builder.Configuration) // <-- add this line
    .WithHttpsHealthCheck("/health");
```

You'll need to provide the `OTEL_EXPORTER_OTLP_ENDPOINT`
value for your service in the `WithOtherOpenTelemetryService` method, and if that
endpoint requires authorization, provide the
`OTEL_EXPORTER_OTLP_HEADERS` value in the user secrets
for the AppHost under the key of `ApmServerAuth`,
something like this:

```json
  "ApmServerAuth": "Authorization=Bearer yourtokenvalue"
```

**N O T E:** This method is provided for simple testing
of an Open Telemetry service and your configuration of
it.  In general, you should use the Aspire Dashboard
locally - it's really fast, and it's only your data - not
your teammates'.  But this method can help you confirm
your deployment setup and configuration before you
actually deploy.
<!--#if ANGULAR-->

## Angular UI

For details about the UI, see the [readme in that folder](./ui-with-bff/README.md).
<!--#endif -->

## Validation

When creating new products or updating existing ones, it's important to
validate the request. The validation is done using the [FluentValidation](https://docs.fluentvalidation.net/en/latest/)
package.  It includes a validation to ensure that no product of the same
name already exists (this involves a call to the database).

Any validation errors are returned as a `400 Bad Request` with details about
the validation failure.

## Versioning

Simple versioning is provided by using attributes on the `Controller` classes or the
methods within them.

The `AddCustomApiVersioning` method defined in `StartupServices/` contains the logic
to add versioning support.  The Swagger extensions also have some logic that ensures
the UI will support them (use the dropdown in the top right of the Swagger UI
to choose a different version).

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
