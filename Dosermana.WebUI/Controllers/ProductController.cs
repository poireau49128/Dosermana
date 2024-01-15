using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dosermana.Domain.Abstract;
using Dosermana.Domain.Entities;
using Dosermana.WebUI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Dosermana.WebUI.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository repository;
        public int pageSize = 15;

        public ProductController(IProductRepository repo)
        {
            repository = repo;
        }

        public ViewResult List(string category, int page = 1)
        {
            decimal Price_coefficient = 1;
            if (User.Identity.IsAuthenticated)
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationContext()));
                var userId = User.Identity.GetUserId();
                var user = userManager.FindById(userId);
                Price_coefficient = user.Price_coefficient;
            }
            ViewBag.Price_coefficient = Price_coefficient;


            ProductsListViewModel model = new ProductsListViewModel
            {
                Products = repository.Products
            .Where(p => category == null || p.Category == category)
            .OrderBy(product => product.ProductId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = category == null ?
        repository.Products.Count() :
        repository.Products.Where(game => game.Category == category).Count()
                },
                CurrentCategory = category
            };
            return View(model);
        }

        //public FileContentResult GetImage(int productId)
        //{
        //    Product product = repository.Products
        //        .FirstOrDefault(g => g.ProductId == productId);

        //    if (product != null)
        //    {
        //        //return File(product.ImageData, product.ImageMimeType);
        //        return null;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
        public FilePathResult GetImage(int productId)
        {
            Product product = repository.Products.FirstOrDefault(g => g.ProductId == productId);

            if (product != null && !string.IsNullOrEmpty(product.FileName))
            {
                string picturePath = Path.Combine(Server.MapPath("~/Images"), product.FileName);
                return File(picturePath, "image/jpeg"); // Adjust the MIME type if necessary
            }
            else
            {
                return null;
            }
        }
    }
}