using Microsoft.Extensions.Configuration;

namespace KnowYourToolset.BackEnd.AppHost;
public static class LoggingHelper
{
    internal static IResourceBuilder<T> WithOtherOpenTelemetryService<T>(this IResourceBuilder<T> builder, IConfiguration config)
        where T : IResourceWithEnvironment
    {
        var apmAuthHeader = config.GetValue<string>("ApmServerAuth"); // put in user secrets

        builder = builder.WithEnvironment("OTEL_EXPORTER_OTLP_ENDPOINT", "YOUR_APM_URL_HERE");
        builder = builder.WithEnvironment("OTEL_EXPORTER_OTLP_HEADERS", apmAuthHeader);
        return builder;
    }
}
