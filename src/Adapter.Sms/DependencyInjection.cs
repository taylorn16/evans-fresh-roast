using Application.Ports;
using Microsoft.Extensions.DependencyInjection;
using Vonage;

namespace Adapter.Sms
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddSmsAdapter(this IServiceCollection services)
        {
            services.AddSingleton<IVonageCredentialsProvider, VonageCredentialsProvider>();
            services.AddTransient(svc => new VonageClient(svc.GetService<IVonageCredentialsProvider>().Get()).SmsClient);
            services.AddTransient<ISendSms, SmsSender>(); // port impl

            return services;
        }
    }
}
