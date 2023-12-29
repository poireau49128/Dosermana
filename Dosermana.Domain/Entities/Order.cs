using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dosermana.Domain.Entities
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public string UserId { get; set; }
        public string UserEmail { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
        public string Address { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal Summary { get; set; }

        public Product Product { get; set; }
        public string GetProductName()
        {
            if (Product != null)
            {
                return Product.Name;
            }
            else
            {
                return "Unknown";
            }
        }
    }
}
