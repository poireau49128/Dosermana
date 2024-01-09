using Microsoft.AspNet.Identity.EntityFramework;

namespace Dosermana.WebUI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Address { get; set; }
        public decimal Price_coefficient { get; set; }
        public ApplicationUser()
        {
            Price_coefficient = 1;
        }
    }
}