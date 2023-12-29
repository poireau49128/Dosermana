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

        //public ViewResult Orders()
        //{
        //    return View(repository_orders.Orders);
        //}
        public ActionResult Orders(string[] status)
        {
            // Получаем все заказы
            var orders = repository_orders.Orders.ToList();

            // Если выбран хотя бы один статус, фильтруем заказы
            if (status != null && status.Length > 0)
            {
                orders = orders.Where(o => status.Contains(o.Status)).ToList();
            }

            return View(orders);
        }

        public ViewResult EditOrder(int orderId)
        {
            Order order = repository_orders.Orders
                .FirstOrDefault(r => r.OrderId == orderId);
            return View(order);
        }
        [HttpPost]
        //public ActionResult EditOrder(int orderId, string newStatus)
        //{
        //    // Получить заказ из базы данных по orderId
        //    var order = repository_orders.Orders.FirstOrDefault(o => o.OrderId == orderId);

        //    if (order != null)
        //    {
        //        // Обновить статус заказа
        //        order.Status = newStatus;
        //        repository_orders.SaveOrder(order);
        //    }

        //    // Перенаправить обратно на страницу "Orders"
        //    return RedirectToAction("Orders");
        //}
        public ActionResult EditOrder(Order order)
        {
            if (ModelState.IsValid)
            {
                repository_orders.SaveOrder(order);
                TempData["message"] = string.Format("Изменения в заказе №\"{0}\" были сохранены", order.OrderId);
                return RedirectToAction("Orders");
            }
            else
            {
                // Что-то не так со значениями данных
                return View(order);
            }
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
                else
                {
                    Product existingProduct = repository.Products.FirstOrDefault(p => p.ProductId == product.ProductId);
                    if (existingProduct != null)
                    {
                        product.ImageData = existingProduct.ImageData;
                        product.ImageMimeType = existingProduct.ImageMimeType;
                    }
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