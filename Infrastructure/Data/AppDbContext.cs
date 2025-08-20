using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Brand> Brands => Set<Brand>();
        public DbSet<Model> Models => Set<Model>();
        public DbSet<Configuration> Configurations => Set<Configuration>();
        public DbSet<Car> Cars => Set<Car>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Sale> Sales => Set<Sale>();
        public DbSet<SaleItem> SaleItems => Set<SaleItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Brand → Model (1:N)
            modelBuilder.Entity<Model>()
                .HasOne(m => m.Brand)
                .WithMany()
                .HasForeignKey(m => m.BrandId);

            // Model → Car (1:N)
            modelBuilder.Entity<Car>()
                .HasOne(c => c.Model)
                .WithMany()
                .HasForeignKey(c => c.ModelId);

            // Brand → Car (1:N)
            modelBuilder.Entity<Car>()
                .HasOne(c => c.Brand)
                .WithMany()
                .HasForeignKey(c => c.BrandId);

            // Configuration → Car (1:N)
            modelBuilder.Entity<Car>()
                .HasOne(c => c.Configuration)
                .WithMany()
                .HasForeignKey(c => c.ConfigurationId);

            // Customer → Sale (1:N)
            modelBuilder.Entity<Sale>()
                .HasOne(s => s.Customer)
                .WithMany()
                .HasForeignKey(s => s.CustomerId);

            // Sale → SaleItem (1:N)
            modelBuilder.Entity<SaleItem>()
                .HasOne(si => si.Sale)
                .WithMany(s => s.Items)
                .HasForeignKey(si => si.SaleId);

            // Car → SaleItem (1:N)
            modelBuilder.Entity<SaleItem>()
                .HasOne(si => si.Car)
                .WithMany()
                .HasForeignKey(si => si.CarId);
        }
    }
}