using System;
using Adapter.Data;
using Adapter.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Invoice = Adapter.Data.Models.Invoice;

namespace WebApp.RegressionTests
{
    internal sealed class SqliteDbContext : ApplicationDbContext
    {
        public SqliteDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Invoice>()
                .Property(i => i.PaymentMethod)
                .HasConversion(pm => pm.ToString(), str => Enum.Parse<Domain.PaymentMethod>(str));

            modelBuilder.Entity<Contact>()
                .HasIndex(c => c.PhoneNumber)
                .IsUnique();

            modelBuilder.Entity<Order>()
                .Property(o => o.CreatedTimestamp)
                .HasDefaultValue(DateTimeOffset.Now)
                .HasConversion(new DateTimeOffsetToStringConverter());

            base.OnModelCreating(modelBuilder);
        }
    }
}
