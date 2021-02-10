using Adapter.Data;
using Microsoft.EntityFrameworkCore;

namespace WebApp.RegressionTests
{
    internal sealed class SqliteDbContextFactory : IDbContextFactory
    {
        public ApplicationDbContext Create()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite("Data source=regression_tests.db;")
                .Options;

            return new SqliteDbContext(options);
        }
    }
}
