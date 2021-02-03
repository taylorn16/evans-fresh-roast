using System.Reflection;
using System.Runtime.CompilerServices;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

[assembly: InternalsVisibleTo("Application.UnitTests")]

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient<ILabelCoffees, CoffeeLabeler>();

            return services;
        }
    }
}
