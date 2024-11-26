using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace KnowYourToolset.BackEnd.ServiceDefaults;
public class UserScopeMiddleware(RequestDelegate next, ILogger<UserScopeMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.User.Identity is { IsAuthenticated: true })
        {
            var user = context.User;
            var subjectId = user.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            var name = user.Claims.FirstOrDefault(c => c.Type == "name")?.Value;
            var email = user.Claims.FirstOrDefault(c => c.Type == "email")?.Value;

            using (logger.BeginScope("User:{subjectId}, Email:{email}, Name:{name}", subjectId, email, name))
            {
                await next(context);
            }
        }
        else
        {
            await next(context);
        }
    }
}
