using System.IdentityModel.Tokens.Jwt;
using KnowYourToolset.BackEnd.Api.StartupServices;
using KnowYourToolset.BackEnd.Api.Swagger;
using KnowYourToolset.BackEnd.ServiceDefaults;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

var builder = WebApplication.CreateBuilder(args);
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

var apiVersionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
app.UseSwaggerFeatures(builder.Configuration, apiVersionProvider, app.Environment);

app.MapDefaultEndpoints();
app
    .UseAuthentication()
    .UseRouting()
    .UseAuthorization()
    .UseMiddleware<UserScopeMiddleware>()
    .UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });

app.Run();