using System.IdentityModel.Tokens.Jwt;
using KnowYourToolset.BackEnd.Api.Data;
using KnowYourToolset.BackEnd.Api.StartupServices;
using KnowYourToolset.BackEnd.Api.Swagger;
using KnowYourToolset.BackEnd.ServiceDefaults;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

var builder = WebApplication.CreateBuilder(args);
builder.AddAppDbContext();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.AddServiceDefaults();

builder.Services.AddControllers();
builder.Services.AddApiProblemDetails();

builder.Services.AddLogic()// defined in StartupServices folder
    .AddCustomApiVersioning()
    .AddSwaggerFeatures()
    .AddHttpContextAccessor();

builder.Services
    .AddMvcCore(options => options.AddBaseAuthorizationFilters())
    .AddApiExplorer();

JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
builder.Services
    .AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration.GetValue<string>("Authentication:Authority");
        options.Audience = builder.Configuration.GetValue<string>("Authentication:ApiName");
    });

var app = builder.Build();
if (args.Contains("--migrateDb")) // use this as a "job" to apply migrations in non-dev
{
    app.ApplyMigrations();
    return 0;
}
if (app.Environment.IsDevelopment()) // do migrations / set up data differently in non-dev
{
    app.SetupDevelopmentDatabase();
}

var apiVersionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
app.UseSwaggerFeatures(builder.Configuration, apiVersionProvider, app.Environment);

app.MapDefaultEndpoints();

app.UseRouting()
    .UseAuthentication()
    .UseMiddleware<UserScopeMiddleware>()
    .UseExceptionHandler() // by putting here we can capture the logged in user in log entry
    .UseAuthorization()
    .UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });

app.Run();
return 0;