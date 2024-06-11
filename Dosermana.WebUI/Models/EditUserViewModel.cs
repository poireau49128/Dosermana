using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dosermana.WebUI.Models
{
    public class EditUserViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public Dictionary<string, decimal> Price_coefficients { get; set; }
        //public decimal FrontCoefficient { get; set; }
        //public decimal MoldingCoefficient { get; set; }
        //public decimal FurnitureCoefficient { get; set; }

    }
}