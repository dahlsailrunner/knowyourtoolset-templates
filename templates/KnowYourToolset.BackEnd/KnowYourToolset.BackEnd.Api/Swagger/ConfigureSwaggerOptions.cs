using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace KnowYourToolset.BackEnd.Api.Swagger
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IConfiguration _config;
        private readonly IApiVersionDescriptionProvider _provider;

        public ConfigureSwaggerOptions(IConfiguration config, IApiVersionDescriptionProvider provider)
        {
            _config = config;
            _provider = provider;
        }

        public void Configure(SwaggerGenOptions options)
        {
            var disco = GetDiscoveryDocument();

            var apiScope = _config.GetValue<string>("Authentication:ApiName");
            var scopes = apiScope.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries).ToList();

            var addScopes = _config.GetValue<string>("Authentication:AdditionalScopes");
            var additionalScopes = addScopes.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            scopes.AddRange(additionalScopes);

            var oauthScopeDic = new Dictionary<string, string>();
            foreach (var scope in scopes)
            {
                oauthScopeDic.Add(scope, $"Resource access: {scope}");
            }

            foreach (var description in _provider.ApiVersionDescriptions)
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
                        AuthorizationUrl = new Uri(disco.AuthorizeEndpoint),
                        TokenUrl = new Uri(disco.TokenEndpoint),
                        Scopes = oauthScopeDic
                    }
                }
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference {Type = ReferenceType.SecurityScheme, Id = "oauth2"}
                    },
                    oauthScopeDic.Keys.ToArray()
                }
            });
        }

        private DiscoveryDocumentResponse GetDiscoveryDocument()
        {
            var client = new HttpClient();
            var authority = _config.GetValue<string>("Authentication:Authority");
            return client.GetDiscoveryDocumentAsync(authority)
                .GetAwaiter()
                .GetResult();
        }
    }
}
