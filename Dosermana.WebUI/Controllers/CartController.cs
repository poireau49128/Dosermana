using System.Linq;
using System.Web.Mvc;
using Dosermana.Domain.Entities;
using Dosermana.Domain.Abstract;
using Dosermana.WebUI.Models;
using Microsoft.AspNet.Identity;

namespace Dosermana.WebUI.Controllers
{
    
    public class CartController : Controller
    {
        private IProductRepository repository;
        private IOrderProcessor orderProcessor;
        public CartController(IProductRepository repo, IOrderProcessor processor)
        {
            repository = repo;
            orderProcessor = processor;
        }

        //[Authorize]
        public RedirectToRouteResult AddToCart(Cart cart, int productId, int quantity, string returnUrl)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            Product product = repository.Products
                .FirstOrDefault(g => g.ProductId == productId);

            if (product != null)
            {
                cart.AddItem(product, quantity);
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

        public ViewResult Checkout()
        {
            return View(new ShippingDetails());
        }

        [HttpPost]
        public ViewResult Checkout(Cart cart, ShippingDetails shippingDetails)
        {
            string userId = User.Identity.GetUserId();
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Извините, ваша корзина пуста!");
            }

            if (ModelState.IsValid)
            {
                orderProcessor.ProcessOrder(cart, shippingDetails, userId);
                cart.Clear();
                return View("Completed");
            }
            else
            {
                return View(shippingDetails);
            }
        }
    }
}