using System.Linq;
using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;

namespace KnowYourToolset.BackEnd.Api.LogEnrichers
{
    public class StandardEnricher :ILogEventEnricher
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public StandardEnricher(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user is {Identity: {IsAuthenticated: true}})
            {
                var sub = user.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
                logEvent.AddPropertyIfAbsent(new LogEventProperty("UserId", new ScalarValue(sub)));
            }
            logEvent.AddPropertyIfAbsent(new LogEventProperty("Something", new ScalarValue("Wild stuff!!!")));
        }
    }
}
