using Adapter.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Vonage.Messaging;

namespace WebApp.RegressionTests
{
    public sealed class TestWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup>
        where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddTransient<IDbContextFactory, SqliteDbContextFactory>();
                services.AddTransient<ISmsClient, LoggerSmsClient>();

                using var scope = services.BuildServiceProvider().CreateScope();
                var serviceProvider = scope.ServiceProvider;
                var db = serviceProvider.GetRequiredService<IDbContextFactory>().Create();

                db.Database.EnsureCreated();
            });
        }
    }
}
