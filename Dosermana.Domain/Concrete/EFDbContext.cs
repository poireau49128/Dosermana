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



        public decimal GetCoefficientForUserAndCategory(string userId, string category)
        {
            var coefficient = UserCategoryCoefficient.FirstOrDefault(c => c.UserId == userId && c.ProductCategory.CategoryName == category);
            return coefficient?.Coefficient ?? 1;
        }
    }
}
