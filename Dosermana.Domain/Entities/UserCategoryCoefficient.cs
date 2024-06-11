using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Dosermana.Domain.Entities
{
    public class UserCategoryCoefficient
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal Coefficient { get; set; } = 1.0m;


        public virtual ProductCategory ProductCategory { get; set; }
    }
}
