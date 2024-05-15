using Dosermana.Domain.Abstract;
using Dosermana.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dosermana.Domain.Concrete
{
    public class EFProductRepository : IProductRepository
    {
        EFDbContext context = new EFDbContext();

        public IEnumerable<Product> Products
        {
            get { return context.Products; }
        }

        public void SaveProduct(Product product)
        {
            if (product.ProductId == 0)
                context.Products.Add(product);
            else
            {
                Product dbEntry = context.Products.Find(product.ProductId);
                if (dbEntry != null)
                {
                    dbEntry.Name = product.Name;
                    dbEntry.Color = product.Color;
                    dbEntry.Quantity_Grodno = product.Quantity_Grodno;
                    dbEntry.Quantity_Moscow = product.Quantity_Moscow;
                    dbEntry.Price = product.Price;
                    dbEntry.Sizes = product.Sizes;
                    dbEntry.Category = product.Category;
                    dbEntry.FileName = product.FileName;
                    dbEntry.Description = product.Description;
                    //dbEntry.ImageData = product.ImageData;
                    //dbEntry.ImageMimeType = product.ImageMimeType;
                }
            }
            context.SaveChanges();
        }

        public Product DeleteProduct(int productId)
        {
            Product dbEntry = context.Products.Find(productId);
            if (dbEntry != null)
            {
                context.Products.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
    }
}
