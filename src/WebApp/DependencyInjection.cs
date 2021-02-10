using Microsoft.Extensions.DependencyInjection;
using WebApp.CoffeeRoastingEvents;

namespace WebApp
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWebApi(this IServiceCollection services)
        {
            services.AddTransient<ICoffeeRoastingEventMapper, CoffeeRoastingEventMapper>();

            return services;
        }
    }
}
