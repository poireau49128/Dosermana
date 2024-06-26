﻿using System.Linq;
using System.Web.Mvc;
using Dosermana.Domain.Entities;
using Dosermana.Domain.Abstract;
using Dosermana.WebUI.Models;
using Microsoft.AspNet.Identity;
using Dosermana.Domain.Concrete;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity;
using System.Collections.Generic;

namespace Dosermana.WebUI.Controllers
{
    
    public class CartController : Controller
    {

        private ApplicationUserManager _userManager;

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


        private IProductRepository repository;
        private IOrderProcessor orderProcessor;
        //private readonly UserManager<ApplicationUser> _userManager;


        private readonly EFDbContext _dbContext;
        public CartController(IProductRepository repo, IOrderProcessor processor, EFDbContext dbcontext/*, UserManager<ApplicationUser> userManager*/)
        {
            repository = repo;
            orderProcessor = processor;
            _dbContext = dbcontext;
            //_userManager = userManager;
        }

        //[Authorize]
        public RedirectToRouteResult AddToCart(Cart cart, int productId, int quantity, string returnUrl, string custom_sizes, string custom_holes)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            Product product = repository.Products
                .FirstOrDefault(g => g.ProductId == productId);

            if (product != null)
            {
                string note = "(размеры: " + custom_sizes + ", отверстия: " + custom_holes + ")";
                cart.AddItem(product, quantity, note);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(Cart cart, int productId, string returnUrl)
        {
            Product product = repository.Products
                .FirstOrDefault(g => g.ProductId == productId);

            if (product != null)
            {
                cart.RemoveLine(product);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public ViewResult Index(Cart cart, string returnUrl)
        {
            var currentUser = UserManager.FindById(User.Identity.GetUserId());
            ViewBag.Price_coefficient = currentUser.Price_coefficient;
            return View(new CartIndexViewModel
            {
                Cart = cart,
                ReturnUrl = returnUrl
            });
        }

        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }

        public ActionResult Orders()
        {
            var userId = User.Identity.GetUserId();
            var currentUser = UserManager.FindById(User.Identity.GetUserId());
            ViewBag.Price_coefficient = currentUser.Price_coefficient;
            using (var dbContext = new EFDbContext())
            {
                //var orders = dbContext.Orders
                //    .Where(o => o.UserId == userId && o.Status != "Выполнен")
                //    .Include(o => o.Product)
                //    .ToList();
                List<Order> orders = dbContext.Orders
                        .Where(o => o.UserId == userId && o.Status != "Выполнен")
                        .Include(o => o.OrderItems)
                        .Include(o => o.OrderItems.Select(oi => oi.Product))
                        .ToList();
                return View(orders);
            }

            //var userId = User.Identity.GetUserId();
            //var userOrders = _dbContext.Orders
            //    .Where(o => o.UserId == userId)
            //    .ToList();
            //return View(userOrders);
        }

        public ViewResult Checkout()
        {
            var currentUser = UserManager.FindById(User.Identity.GetUserId());

            CurrentUser user = new CurrentUser
            {
                Id = currentUser.Id,
                Email = currentUser.Email,
                Address = currentUser.Address,
                Price_coefficient = currentUser.Price_coefficient,
            };
            return View(user);
        }

        [HttpPost]
        public ViewResult Checkout(Cart cart, CurrentUser user)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Извините, ваша корзина пуста!");
            }

            if (ModelState.IsValid)
            {
                bool flag = orderProcessor.ProcessOrder(cart, user);
                cart.Clear();
                if (flag)
                        return View("Completed");
                else
                        return View("CheckoutWarning");
            }
            else
            {
                return View(user);
            }
        }
    }
}