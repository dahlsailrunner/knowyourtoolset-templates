using FluentValidation;
using KnowYourToolset.BackEnd.Api.BusinessLogic;

namespace KnowYourToolset.BackEnd.Api.StartupServices;

public static class LogicExtensions
{
    public static IServiceCollection AddLogic(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(ProductBusinessLogic).Assembly);
        services.AddAutoMapper(typeof(ProductBusinessLogic).Assembly);

        services.AddScoped<IProductBusinessLogic, ProductBusinessLogic>();
        services.AddScoped<IPostalCodeLogic, PostalCodeLogic>();
        return services;
    }
}
