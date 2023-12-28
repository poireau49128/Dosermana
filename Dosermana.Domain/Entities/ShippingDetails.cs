using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dosermana.Domain.Entities
{
    public class ShippingDetails
    {
        [Required(ErrorMessage = "Укажите ваше имя")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Укажите адрес доставки")]
        [Display(Name = "Адрес доставки")]
        public string Address { get; set; }
    }
}
