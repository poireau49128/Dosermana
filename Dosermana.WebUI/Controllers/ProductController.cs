using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dosermana.Domain.Abstract;
using Dosermana.Domain.Concrete;
using Dosermana.Domain.Entities;
using Dosermana.WebUI.Models;
using System.Web.Caching;

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

        public ViewResult List(string subcategory = "ПН-37", string category = "Погонаж", int page = 1)
        {
            decimal priceCoefficient = 1;
            if (User.Identity.IsAuthenticated)
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationContext()));
                var userId = User.Identity.GetUserId();
                var user = userManager.FindById(userId);
                using (var dbContext = new EFDbContext())
                {
                    priceCoefficient = dbContext.GetCoefficientForUserAndCategory(userId, category);
                }
            }
            ViewBag.Price_coefficient = priceCoefficient;


            ProductsListViewModel model = new ProductsListViewModel
            {
                Products = repository.Products
            .Where(p => p.Category == category && p.SubCategory == subcategory)
            .OrderBy(product => product.ProductId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = category == null ?
        repository.Products.Count() :
        repository.Products.Where(product => product.SubCategory == subcategory).Count()
                },
                CurrentCategory = category
            };
            return View(model);
        }

        public ViewResult Details(int productId)
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

            Product product = repository.Products
                .FirstOrDefault(p => p.ProductId == productId);
            return View(product);
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


        //private string GetFirstCategory()
        //{
        //    var cacheKey = "FirstCategory";
        //    var cachedValue = HttpRuntime.Cache.Get(cacheKey) as string;

        //    if (cachedValue == null)
        //    {
        //        cachedValue = repository.Products
        //            .OrderBy(p => p.Category)
        //            .Select(p => p.Category)
        //            .FirstOrDefault();

        //        HttpRuntime.Cache.Insert(cacheKey, cachedValue, null, DateTime.Now.AddHours(1), Cache.NoSlidingExpiration);
        //    }

        //    return cachedValue;
        //}

        //private string GetFirstSubcategory()
        //{
        //    var cacheKey = "FirstSubcategory";
        //    var cachedValue = HttpRuntime.Cache.Get(cacheKey) as string;

        //    if (cachedValue == null)
        //    {
        //        var firstCategory = GetFirstCategory();

        //        cachedValue = repository.Products
        //            .Where(p => p.Category == firstCategory)
        //            .OrderBy(p => p.SubCategory)
        //            .Select(p => p.SubCategory)
        //            .FirstOrDefault();

        //        HttpRuntime.Cache.Insert(cacheKey, cachedValue, null, DateTime.Now.AddHours(1), Cache.NoSlidingExpiration);
        //    }

        //    return cachedValue;
        //}
    }
}