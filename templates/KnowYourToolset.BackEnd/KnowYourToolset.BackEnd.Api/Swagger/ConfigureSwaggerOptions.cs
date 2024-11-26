using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace KnowYourToolset.BackEnd.Api.Swagger;

public class ConfigureSwaggerOptions(IConfiguration config, IApiVersionDescriptionProvider provider) 
    : IConfigureOptions<SwaggerGenOptions>
{
    public void Configure(SwaggerGenOptions options)
    {
        var authority = config.GetValue<string>("Authentication:Authority");

        var apiScope = config.GetValue<string>("Authentication:ApiName");
        var scopes = apiScope!.Split([' '], StringSplitOptions.RemoveEmptyEntries).ToList();

        var addScopes = config.GetValue<string>("Authentication:AdditionalScopes");
        if (!string.IsNullOrEmpty(addScopes))
        {
            var additionalScopes = addScopes.Split([' '], StringSplitOptions.RemoveEmptyEntries).ToList();
            scopes.AddRange(additionalScopes);
        }

        var oauthScopeDic = new Dictionary<string, string>();
        foreach (var scope in scopes)
        {
            oauthScopeDic.Add(scope, $"Resource access: {scope}");
        }

        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(
                description.GroupName,
                new OpenApiInfo
                {
                    Title = $"KnowYourToolset_BackEnd {description.ApiVersion}",
                    Version = description.ApiVersion.ToString(),
                });
        }

        options.EnableAnnotations();

        options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.OAuth2,
            Flows = new OpenApiOAuthFlows
            {
                AuthorizationCode = new OpenApiOAuthFlow
                {
                    AuthorizationUrl = new Uri($"{authority}/connect/authorize"),
                    TokenUrl = new Uri($"{authority}/connect/token"),
                    Scopes = oauthScopeDic
                }
            }
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
                },
                oauthScopeDic.Keys.ToArray()
            }
        });
    }
}
