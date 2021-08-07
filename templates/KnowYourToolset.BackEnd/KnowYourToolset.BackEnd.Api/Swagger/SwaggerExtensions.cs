using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace KnowYourToolset.BackEnd.Api.Swagger
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddSwaggerFeatures(this IServiceCollection services)
        {
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen();

            return services;
        }

        public static IApplicationBuilder UseSwaggerFeatures(this IApplicationBuilder app, IConfiguration config, IApiVersionDescriptionProvider provider, IWebHostEnvironment env)
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
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", $"KnowYourToolset_BackEnd API {description.GroupName.ToUpperInvariant()}");
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
}
