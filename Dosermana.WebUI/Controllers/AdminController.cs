using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dosermana.Domain.Abstract;
using Dosermana.Domain.Entities;

namespace Dosermana.WebUI.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        IProductRepository repository;
        IOrderRepository repository_orders;

        public AdminController(IProductRepository repo, IOrderRepository orders)
        {
            repository = repo;
            repository_orders = orders;
        }

        public ViewResult Index()
        {
            return View(repository.Products);
        }

        public ViewResult Orders()
        {
            return View(repository_orders.Orders);
        }

        public ViewResult Edit(int productId)
        {
            Product product = repository.Products
                .FirstOrDefault(p => p.ProductId == productId);
            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(Product product, HttpPostedFileBase image = null)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    product.ImageMimeType = image.ContentType;
                    product.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(product.ImageData, 0, image.ContentLength);
                }
                repository.SaveProduct(product);
                TempData["message"] = string.Format("Изменения \"{0} {1}\" были сохранены", product.Name, product.Color);
                return RedirectToAction("Index");
            }
            else
            {
                // Что-то не так со значениями данных
                return View(product);
            }
        }

        public ViewResult Create()
        {
            return View("Edit", new Product());
        }

        [HttpPost]
        public ActionResult Delete(int productId)
        {
            Product deletedProduct = repository.DeleteProduct(productId);
            if (deletedProduct != null)
            {
                TempData["message"] = string.Format("\"{0} {1}\" был удален",
                    deletedProduct.Name, deletedProduct.Color);
            }
            return RedirectToAction("Index");
        }


        [HttpPost]
        public ActionResult UploadImage(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var filePath = Path.Combine(Server.MapPath("~/Images/"), fileName);
                file.SaveAs(filePath);

                return Json(new { fileName });
            }

            return HttpNotFound();
        }
    }
}