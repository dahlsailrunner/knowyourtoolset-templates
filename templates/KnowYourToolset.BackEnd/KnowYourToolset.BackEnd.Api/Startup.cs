using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using KnowYourToolset.ApiComponents.Middleware;
using KnowYourToolset.BackEnd.Api.Extensions;
using KnowYourToolset.BackEnd.Api.LogEnrichers;
using KnowYourToolset.BackEnd.Api.Swagger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Serilog;
using Serilog.Core;

namespace KnowYourToolset.BackEnd.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
            services.AddHealthChecks();

            services.AddLogic();// defined in Extensions folder
            services.AddCustomApiVersioning();
            services.AddSwaggerFeatures();
            services.AddTransient<ILogEventEnricher, StandardEnricher>();
            services.AddHttpContextAccessor();

            services
                .AddMvcCore(options => { options.AddBaseAuthorizationFilters(Configuration); })
                .AddCors()
                .AddApiExplorer();

            services
                .AddAuthentication("Bearer")
                .AddJwtBearer(options =>
                {
                    options.Authority = Configuration.GetValue<string>("Authentication:Authority");
                    options.Audience = Configuration.GetValue<string>("Authentication:ApiName");
                });
        }

        public void Configure(IApplicationBuilder app, IApiVersionDescriptionProvider provider, IWebHostEnvironment env)
        {
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

            app.UseSwaggerFeatures(Configuration, provider, env);
            app.UseAuthentication();
            app.UseCustomRequestLogging();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
        }

        private HttpStatusCode CustomizeResponse(HttpContext httpContext, Exception exception, ProblemDetails problemDetails)
        {
            if (exception is ApplicationException appEx)
            {
                problemDetails.Detail = appEx.Message;
                return HttpStatusCode.BadRequest;
            }
            return HttpStatusCode.InternalServerError; 
        }
    }
}
