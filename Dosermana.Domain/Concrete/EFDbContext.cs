using Dosermana.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dosermana.Domain.Concrete
{
    public class EFDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<UserCategoryCoefficient> UserCategoryCoefficient { get; set; }
        public DbSet<ProductCategory> ProductCategory { get; set; }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Order>()
        //        .HasRequired(o => o.Product)
        //        .WithMany()
        //        .HasForeignKey(o => o.ProductId);
        //}
    }
}
