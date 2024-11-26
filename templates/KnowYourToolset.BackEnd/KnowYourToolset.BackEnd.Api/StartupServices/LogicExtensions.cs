using KnowYourToolset.BackEnd.Api.BusinessLogic;

namespace KnowYourToolset.BackEnd.Api.StartupServices;

public static class LogicExtensions
{
    public static IServiceCollection AddLogic(this IServiceCollection services)
    {
        services.AddScoped<IPostalCodeLogic, PostalCodeLogic>();
        return services;
    }
}
