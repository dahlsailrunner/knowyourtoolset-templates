using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace KnowYourToolset.BackEnd.Api.Swagger;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerFeatures(this IServiceCollection services)
    {
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        services.AddSwaggerGen(o => o.OperationFilter<ProblemDetailsResponseOperationFilter>());

        return services;
    }

    public static IApplicationBuilder UseSwaggerFeatures(this IApplicationBuilder app, IConfiguration config,
        IApiVersionDescriptionProvider provider, IWebHostEnvironment env)
    {
        if (!env.IsDevelopment())
        {
            return app;
        }

        var clientId = config.GetValue<string>("Authentication:SwaggerClientId");
        app
            .UseSwagger()
            .UseSwaggerUI(options =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                        $"KnowYourToolset_BackEnd API {description.GroupName.ToUpperInvariant()}");
                    options.RoutePrefix = string.Empty;
                }

                options.DocumentTitle = "KnowYourToolset_BackEnd Documentation";
                options.OAuthClientId(clientId);
                options.OAuthAppName("KnowYourToolset_BackEnd");
                options.OAuthUsePkce();
            });

        return app;
    }
}

public class ProblemDetailsResponseOperationFilter : IOperationFilter 
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var problemDetailsSchema = context.SchemaGenerator.GenerateSchema(typeof(ProblemDetails), context.SchemaRepository);

        operation.Responses.Add("500", new OpenApiResponse
        {
            Description = "Internal Server Error",
            Content = new Dictionary<string, OpenApiMediaType>
            {
                ["application/json"] = new OpenApiMediaType
                {
                    Schema = problemDetailsSchema
                }
            }
        });
        operation.Responses.Add("400", new OpenApiResponse
        {
            Description = "Bad Request",
            Content = new Dictionary<string, OpenApiMediaType>
            {
                ["application/json"] = new OpenApiMediaType
                {
                    Schema = problemDetailsSchema
                }
            }
        });
    }
}