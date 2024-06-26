﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Dosermana.Domain.Abstract;
using Dosermana.Domain.Concrete;
using Dosermana.Domain.Entities;
using Dosermana.WebUI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Dosermana.WebUI.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        IProductRepository repository;
        IOrderRepository repository_orders;
        private ApplicationUserManager _userManager;

        private readonly EFDbContext _dbContext;
        public ApplicationUserManager UserManager
        {
            get
            {
                if (_userManager == null)
                {
                    _userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                }
                return _userManager;
            }
        }

        public AdminController(IProductRepository repo, IOrderRepository orders, EFDbContext dbcontext)
        {
            repository = repo;
            repository_orders = orders;
            _dbContext = dbcontext;
        }

        public ViewResult Index()
        {
            return View(repository.Products);
        }

        public ActionResult Users()
        {
            UserManager<ApplicationUser> _userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            return View(_userManager.Users.ToList());
        }


        public async Task<ViewResult> EditUser(string userID)
        {
            ApplicationUser user = await UserManager.FindByIdAsync(userID);
            if (user == null)
            {
                return null;
            }
            EditUserViewModel model = new EditUserViewModel { Id = user.Id, Name = user.UserName, Address = user.Address, Price_coefficient = user.Price_coefficient };
            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> EditUser(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await UserManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    user.Email = model.Name;
                    user.UserName = model.Name;
                    user.Address = model.Address;
                    user.Price_coefficient = model.Price_coefficient;

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        TempData["message"] = string.Format("Изменения пользователя {0} были сохранены", model.Name);
                        return RedirectToAction("Users");
                    }
                    else
                    {
                        return View(model);
                    }
                }
            }
            return View(model);
        }

        public ActionResult Orders(string[] status)
        {
            using (var dbContext = new EFDbContext())
            {
                List<Order> orders;

                

                if (status != null && status.Length > 0)
                {
                    orders = dbContext.Orders
                        .Where(o => status.Contains(o.Status))
                        .Include(o => o.OrderItems)
                        .Include(o => o.OrderItems.Select(oi => oi.Product))
                        .ToList();

                    ViewBag.T = 123;
                }
                else
                {
                    orders = dbContext.Orders
                        .Include(o => o.OrderItems)
                        .Include(o => o.OrderItems.Select(oi => oi.Product))
                        .ToList();
                }

                // Export to Excel if requested
                if (Request.QueryString["export"] == "excel")
                {
                    return ExportToExcel(status); // Передайте параметры фильтрации в метод ExportToExcel
                }

                return View(orders);
            }
        }

        private ActionResult ExportToExcel(string[] status)
        {
            using (var dbContext = new EFDbContext())
            {
                List<Order> orders;

                // Применить фильтрацию, если есть параметры
                if (status != null && status.Length > 0 && status[0] != "")
                {
                    status = status[0].Split(',');
                    orders = dbContext.Orders
                        .Where(o => status.Contains(o.Status))
                        .Include(o => o.OrderItems)
                        .Include(o => o.OrderItems.Select(oi => oi.Product))
                        .ToList();
                }
                else
                {
                    orders = dbContext.Orders
                        .Include(o => o.OrderItems)
                        .Include(o => o.OrderItems.Select(oi => oi.Product))
                        .ToList();
                }

                // Остальной код экспорта в Excel остается без изменений
                // ...

                var package = new ExcelPackage();
                var worksheet = package.Workbook.Worksheets.Add("Orders");

                // Set column headers
                worksheet.Cells[1, 1].Value = "Номер заказа";
                worksheet.Cells[1, 2].Value = "Имя пользователя";
                worksheet.Cells[1, 3].Value = "Продукты";
                worksheet.Cells[1, 4].Value = "Количество";
                worksheet.Cells[1, 5].Value = "Общая стоимость";
                worksheet.Cells[1, 6].Value = "Дата заказа";
                worksheet.Cells[1, 7].Value = "Адрес";
                worksheet.Cells[1, 8].Value = "Статус заказа";
                worksheet.Cells[1, 9].Value = "Примечание";
                // Add more columns as needed

                // Populate data rows
                int currentRow = worksheet.Dimension.End.Row + 1;
                foreach (var order in orders)
                {
                    var orderStartRow = currentRow;

                    worksheet.Cells[orderStartRow, 1].Value = order.OrderId;
                    worksheet.Cells[orderStartRow, 2].Value = order.UserEmail;


                    


                    foreach (var orderItem in order.OrderItems)
                    {
                        worksheet.Cells[currentRow, 3].Value = orderItem.Product.Name + " " + orderItem.Product.Color;
                        worksheet.Cells[currentRow, 4].Value = orderItem.Quantity;
                        currentRow++;
                    }
                    


                    worksheet.Cells[orderStartRow, 5].Value = order.Summary;

                    ExcelRange cell = worksheet.Cells[orderStartRow, 6];
                    cell.Style.Numberformat.Format = "dd.mm.yyyy hh:mm:ss";
                    worksheet.Cells[orderStartRow, 6].Value = order.OrderDate;


                    worksheet.Cells[orderStartRow, 7].Value = order.Address;
                    worksheet.Cells[orderStartRow, 8].Value = order.Status;
                    worksheet.Cells[orderStartRow, 9].Value = order.Note;

                    currentRow++;
                    // Add more data columns as needed
                }

                worksheet.Cells.AutoFitColumns();

                // Set response headers for file download
                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=Orders.xlsx");
                Response.BinaryWrite(package.GetAsByteArray());
                Response.End();

                return new EmptyResult(); // Вернуть пустой результат, чтобы представление не рендерилось
            }
        }


        public string GetProductById(int productId)
        {
            Product product = repository.Products
                .FirstOrDefault(p => p.ProductId == productId);
            if (product != null)
            {
                return $"{product.Name} {product.Color}";
            }

            return string.Empty;
        }

        public ViewResult EditOrder(int orderId)
        {
            Order order = repository_orders.Orders
                .FirstOrDefault(r => r.OrderId == orderId);
            return View(order);
        }
        [HttpPost]
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

        [HttpPost]
        public ActionResult DeleteOrder(int orderId)
        {
            Order deletedOrder = repository_orders.DeleteOrder(orderId);
            if (deletedOrder != null)
            {
                TempData["message"] = string.Format("Заказ №\"{0}\" был удален", deletedOrder.OrderId);
            }
            return RedirectToAction("Orders");
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
                    product.FileName = UploadImage(image);
                    //product.ImageMimeType = image.ContentType;
                    //product.ImageData = new byte[image.ContentLength];
                    //image.InputStream.Read(product.ImageData, 0, image.ContentLength);
                }
                else
                {
                    Product existingProduct = repository.Products.FirstOrDefault(p => p.ProductId == product.ProductId);
                    if (existingProduct != null)
                    {
                        product.FileName = existingProduct.FileName;
                        //product.ImageMimeType = existingProduct.ImageMimeType;
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


        //[HttpPost]
        //public ActionResult UploadImage(HttpPostedFileBase file)
        //{
        //    if (file != null && file.ContentLength > 0)
        //    {
        //        var fileName = Path.GetFileName(file.FileName);
        //        var filePath = Path.Combine(Server.MapPath("~/Images/"), fileName);
        //        file.SaveAs(filePath);

        //        return Json(new { fileName });
        //    }

        //    return HttpNotFound();
        //}
        public string UploadImage(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var filePath = Path.Combine(Server.MapPath("~/Images/"), fileName);
                file.SaveAs(filePath);

                return fileName;
            }

            return null;
        }
    }
}