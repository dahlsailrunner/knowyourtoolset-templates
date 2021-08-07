using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;

namespace KnowYourToolset.BackEnd.Api.Extensions
{
    public static class AuthorizationExtensions
    {
        public static MvcOptions AddBaseAuthorizationFilters(this MvcOptions options, IConfiguration config)
        {
            var builder = new AuthorizationPolicyBuilder().RequireAuthenticatedUser();
            options.Filters.Add(new AuthorizeFilter(builder.Build()));

            return options;
        }
    }
}
