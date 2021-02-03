using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<ICurrentTimeProvider, CurrentTimeProvider>();
            services.AddTransient<ILocalDateHumanizer, LocalDateHumanizer>();

            return services;
        }
    }
}
