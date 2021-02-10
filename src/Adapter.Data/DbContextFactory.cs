using Microsoft.EntityFrameworkCore;

namespace Adapter.Data
{
    internal interface IDbContextFactory
    {
        ApplicationDbContext Create();
    }

    internal sealed class DbContextFactory : IDbContextFactory
    {
        public ApplicationDbContext Create()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer("") // TODO get connection string from env/configuration
                .Options;

            return new ApplicationDbContext(options);
        }
    }
}
