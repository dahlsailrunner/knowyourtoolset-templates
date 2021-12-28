using System.IdentityModel.Tokens.Jwt;
using System.Net;
using KnowYourToolset.ApiComponents.Middleware;
using KnowYourToolset.BackEnd.Api.LogEnrichers;
using KnowYourToolset.BackEnd.Api.StartupServices;
using KnowYourToolset.BackEnd.Api.Swagger;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Serilog;
using Serilog.Core;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up");

try
{
    

    var builder = WebApplication.CreateBuilder(args);
    var configuration = builder.Configuration;
   
    builder.Services.AddControllers();
    builder.Services.AddHealthChecks();

    builder.Services.AddLogic()// defined in StartupServices folder
        .AddCustomApiVersioning()
        .AddSwaggerFeatures()
        .AddTransient<ILogEventEnricher, StandardEnricher>()
        .AddHttpContextAccessor();

    builder.Services
        .AddMvcCore(options => { options.AddBaseAuthorizationFilters(configuration); }) //                .AddCors()
        .AddApiExplorer();

    JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
    builder.Services
        .AddAuthentication("Bearer")
        .AddJwtBearer(options =>
        {
            options.Authority = configuration.GetValue<string>("Authentication:Authority");
            options.Audience = configuration.GetValue<string>("Authentication:ApiName");
        });

    builder.Host.UseSerilog(((context, services, loggerConfig) =>
    {
        loggerConfig
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Application", "KnowYourToolset_BackEnd.Api") // or entry assembly name
            .WriteTo.Console()
            //.WriteTo.Seq("http://host.docker.internal:5341");  // comment this IN if using docker
            .WriteTo.Seq("http://localhost:5341");       // comment this OUT if using using docker);
    }));

    var app = builder.Build();


    // Configure the HTTP request pipeline.
    app.UseProblemDetailsHandler(options => options.AddResponseDetails = CustomizeResponse);

    if (Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true")
    {
        var forwardedHeaderOptions = new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        };
        forwardedHeaderOptions.KnownNetworks.Clear();
        forwardedHeaderOptions.KnownProxies.Clear();
        app.UseForwardedHeaders(forwardedHeaderOptions);
    }

    var corsOrigins = configuration.GetValue<string>("CORSOrigins")?.Split(",");
    if (corsOrigins!= null && corsOrigins.Any())
    {
        app.UseCors(bld => bld
            .WithOrigins(corsOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod());
    }

    var apiVersionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

    app
        .UseSwaggerFeatures(configuration, apiVersionProvider, app.Environment)
        .UseAuthentication()
        .UseCustomRequestLogging()
        .UseRouting()
        .UseAuthorization()
        .UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHealthChecks("/health");
            endpoints.MapFallback(() => Results.Redirect("/swagger"));
        });

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}


HttpStatusCode CustomizeResponse(HttpContext ctx, Exception ex, ProblemDetails problemDetails)
{
    var httpStatus = HttpStatusCode.InternalServerError;
    if (ex is ApplicationException appEx)
    {
        problemDetails.Detail = appEx.Message;
        httpStatus = HttpStatusCode.BadRequest;
    }

    return httpStatus;
}






//Log.Logger = new LoggerConfiguration()
//                .Enrich.FromLogContext()
//                .WriteTo.Console()
//                .CreateBootstrapLogger();

//            try
//            {
//                Log.Information("Starting web host");
//                CreateHostBuilder(args).Build().Run();
//            }
//            catch (Exception ex)
//            {
//                Log.Fatal(ex, "Host terminated unexpectedly");
//            }
//            finally
//            {
//                Log.CloseAndFlush();
//            }
//        }

//        public static IHostBuilder CreateHostBuilder(string[] args) =>
//            Host.CreateDefaultBuilder(args)
//                .UseSerilog((context, services, configuration) =>
//                    configuration
//                        .ReadFrom.Configuration(context.Configuration)
//                        .ReadFrom.Services(services)
//                        .Enrich.FromLogContext()
//                        .Enrich.WithProperty("Application", "KnowYourToolset_BackEnd.Api") // or entry assembly name
//                        .WriteTo.Console()
//                        //.WriteTo.Seq("http://host.docker.internal:5341"))  // comment this IN if using docker
//                        .WriteTo.Seq("http://localhost:5341"))  // comment this OUT if using using docker
//                .ConfigureWebHostDefaults(webBuilder =>
//                {
//                    webBuilder.UseStartup<Startup>();
//                });
//    }
//}
