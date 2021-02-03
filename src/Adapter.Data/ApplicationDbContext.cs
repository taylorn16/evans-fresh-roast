using System;
using Domain;
using Microsoft.EntityFrameworkCore;
using Invoice = Adapter.Data.Models.Invoice;

namespace Adapter.Data
{
    internal sealed class ApplicationDbContext : DbContext
    {
        public DbSet<Adapter.Data.Models.CoffeeRoastingEvent> CoffeeRoastingEvents { get; set; }
        public DbSet<Adapter.Data.Models.Contact> Contacts { get; set; }
        public DbSet<Adapter.Data.Models.Coffee> Coffees { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Invoice>()
                .Property(i => i.PaymentMethod)
                .HasConversion(pm => pm.ToString(), str => Enum.Parse<PaymentMethod>(str));

            modelBuilder.Entity<Adapter.Data.Models.Contact>()
                .HasIndex(c => c.PhoneNumber)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
