using System.Reflection;
using System.Runtime.CompilerServices;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Application.Behaviors;
using Application.Orders;
using Domain.Exceptions;
using MediatR.Pipeline;

[assembly: InternalsVisibleTo("Application.UnitTests")]

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CommandValidatorBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestExceptionProcessorBehavior<,>));
            AddCommandValidators(services);

            services.AddTransient<ILabelCoffees, CoffeeLabeler>();
            services.AddTransient<
                IRequestExceptionHandler<PlaceOrderCommand, Unit, DomainBehaviorException>,
                PlaceOrderDomainBehaviorExceptionHandler>();

            return services;
        }

        private static void AddCommandValidators(IServiceCollection services)
        {
            var validatorRegistrations = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => !t.IsAbstract)
                .Where(t => !t.IsGenericType)
                .Where(t => !t.IsInterface)
                .SelectMany(@class =>
                {
                    var interfaces = @class.GetInterfaces().Where(i =>
                        i.IsGenericType &&
                        i.GetGenericTypeDefinition() == typeof(ICommandValidator<>));

                    return interfaces.Select(@interface => (@interface, @class));
                })
                .Where(x => x.@interface != null);

            foreach (var (@interface, @class) in validatorRegistrations)
            {
                services.AddTransient(@interface, @class);
            }
        }
    }
}
