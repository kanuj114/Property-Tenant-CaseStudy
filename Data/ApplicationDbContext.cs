using Microsoft.EntityFrameworkCore;
using PropertyUI.Models;

namespace PropertyUI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Property>()
                .HasMany(t => t.Tenants)
                .WithOne(t => t.MyProperty)
                .HasForeignKey(t => t.PropertyId)
                .IsRequired();

        }

        public DbSet<Property> Properties { get; set; }
        public DbSet<Tenant> Tenants { get; set; }
    }
}

