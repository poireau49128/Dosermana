using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dosermana.Domain.Entities
{
    public class Cart
    {
        private List<CartLine> lineCollection = new List<CartLine>();

        public void AddItem(Product product, int quantity, string note)
        {
            CartLine line = lineCollection
                .Where(p => p.Product.ProductId == product.ProductId)
                .FirstOrDefault();

            if (line == null)
            {
                product.Name = product.Name + note;
                lineCollection.Add(new CartLine
                {
                    
                    Product = product,
                    
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public void RemoveLine(Product product)
        {
            lineCollection.RemoveAll(l => l.Product.ProductId == product.ProductId);
        }

        public decimal ComputeTotalValue(Dictionary<string, decimal> coefs)
        {
            return lineCollection.Sum(e => e.Product.Price * e.Quantity * coefs[e.Product.Category]);

        }
        public void Clear()
        {
            lineCollection.Clear();
        }

        public IEnumerable<CartLine> Lines
        {
            get { return lineCollection; }
        }
    }

    public class CartLine
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal Price_coefficient { get; set; }
    }
}
