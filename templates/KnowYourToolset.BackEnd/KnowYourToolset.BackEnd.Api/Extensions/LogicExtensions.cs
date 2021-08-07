using KnowYourToolset.BackEnd.Logic;
using Microsoft.Extensions.DependencyInjection;

namespace KnowYourToolset.BackEnd.Api.Extensions
{
    public static class LogicExtensions
    {
        public static IServiceCollection AddLogic(this IServiceCollection services)
        {
            services.AddScoped<IPostalCodeLogic, PostalCodeLogic>();
            return services;
        }
    }
}
