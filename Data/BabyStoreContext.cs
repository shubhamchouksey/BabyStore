using Microsoft.EntityFrameworkCore;
using BabyStore.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using BabyStore.ViewModel;
using BabyStore.Data;
using Microsoft.AspNetCore.Identity;

namespace BabyStore.Models
{

    public class BabyStoreContext : IdentityDbContext<ApplicationUser>
    {
        public BabyStoreContext (DbContextOptions<BabyStoreContext> options)
            : base(options)
        {
        }
        public DbSet<Product> Product { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<ProductImage> ProductImage { get; set; }
        public DbSet<ProductImageMapping> ProductImageMapping { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<BasketLine> BasketLine { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderLine> OrderLine { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductImage>()
                .HasIndex(p => p.FileName).IsUnique(true);
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Address>()
            .HasOne(a => a.ApplicationUser)
            .WithOne(b => b.Address)
            .HasForeignKey<ApplicationUser>(b => b.UserRef);

        }
        
    }
}
