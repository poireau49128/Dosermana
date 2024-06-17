using Dosermana.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dosermana.WebUI.Models
{
    public class EditUserViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public List<CategoryCoefficientViewModel> CategoryCoefficients { get; set; }
        public List<ProductCategory> Categories { get; set; }
    }

    public class CategoryCoefficientViewModel
    {
        public int Category_Id { get; set; }
        public decimal Coefficient { get; set; }
    }
}