using Microsoft.AspNet.Identity.EntityFramework;

namespace Dosermana.WebUI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Address { get; set; }
        public ApplicationUser()
        {
        }
    }
}