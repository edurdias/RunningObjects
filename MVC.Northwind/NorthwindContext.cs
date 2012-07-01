using System.Configuration;
using System.Data.Entity;
using RunningObjects.MVC.Data;
using RunningObjects.MVC.Northwind.Products;
using RunningObjects.MVC.Northwind.Sales;

namespace RunningObjects.MVC.Northwind
{
    public class NorthwindContext : RunningObjectsDbContext
    {
        public NorthwindContext()
            : base("Northwind")
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Territory> Territories { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasKey(c => c.CategoryID)
                .HasMany(c => c.Products)
                .WithRequired(p => p.Category)
                .HasForeignKey(p => p.CategoryID);

            modelBuilder.Entity<Region>()
                .HasKey(r => r.RegionID)
                .HasMany(r => r.Territories)
                .WithRequired(t => t.Region)
                .HasForeignKey(t => t.RegionID);

            base.OnModelCreating(modelBuilder);
        }
    }
}
