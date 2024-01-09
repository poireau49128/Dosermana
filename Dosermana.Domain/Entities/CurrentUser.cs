using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dosermana.Domain.Entities
{
    public class CurrentUser
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public decimal Price_coefficient { get; set; }
    }
}
