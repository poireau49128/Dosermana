using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Dosermana.Domain.Abstract;

namespace Dosermana.WebUI.Controllers
{
    public class NavController : Controller
    {
        private IProductRepository repository;
        public NavController(IProductRepository repo)
        {
            repository = repo;
        }
        public PartialViewResult Menu(string category = null, bool horizontalNav = false)
        {
            ViewBag.SelectedCategory = category;

            IEnumerable<string> categories = repository.Products
                .Select(product => product.Category)
                .Distinct()
                .OrderBy(x => x);
            return PartialView("FlexMenu", categories);
        }
    }
}