using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Dosermana.Domain.Entities
{
    public class Order
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int OrderId { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string UserId { get; set; }

        [Display(Name = "Имя пользователя")]
        public string UserEmail { get; set; }

        [Display(Name = "ИД продукта")]
        public int ProductId { get; set; }

        [Display(Name = "Количество")]
        public int Quantity { get; set; }

        [Display(Name = "Статус заказа")]
        public string Status { get; set; }

        [Display(Name = "Адрес доставки")]
        public string Address { get; set; }

        [Display(Name = "Дата заказа")]
        public DateTime OrderDate { get; set; }

        [Display(Name = "Общая стоимость")]
        public decimal Summary { get; set; }

        [HiddenInput(DisplayValue = false)]
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
