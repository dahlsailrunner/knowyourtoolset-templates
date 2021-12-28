namespace KnowYourToolset.BackEnd.Api.StartupServices;

public static class ApiVersioningExtensions
{
    public static IServiceCollection AddCustomApiVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(
            options => { options.ReportApiVersions = true; });
        services.AddVersionedApiExplorer(
            options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

        return services;
    }
}
