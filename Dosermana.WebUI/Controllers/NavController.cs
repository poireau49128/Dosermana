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
        public PartialViewResult Menu(string selectedCategory = null, bool horizontalNav = false)
        {
            ViewBag.SelectedCategory = selectedCategory;

            Dictionary<string, List<string>> categoryDictionary = new Dictionary<string, List<string>>();

            var categories = repository.Products
                .Select(product => product.Category)
                .Distinct()
                .OrderBy(x => x);

            foreach (var category in categories)
            {
                var subcategories = repository.Products
                    .Where(product => product.Category == category)
                    .Select(product => product.SubCategory)
                    .Distinct()
                    .OrderBy(x => x)
                    .ToList();

                categoryDictionary.Add(category, subcategories);
            }
            return PartialView("FlexMenu", categoryDictionary);
        }

        public ViewResult WhereToBuy()
        {
            return View();
        }
        public ViewResult About()
        {
            return View();
        }
    }
}