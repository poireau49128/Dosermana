using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Dosermana.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Маршрут для главной страницы
            routes.MapRoute(null,
                "",
                new
                {
                    controller = "Product",
                    action = "List",
                    category = (string)null,
                    subcategory = (string)null,
                    page = 1
                }
            );


            routes.MapRoute(null,
                "{category}/{page}",
                new { controller = "Product", action = "List" },
                new { page = @"\d+" }
            );
            // Маршрут для отображения продуктов по категории, подкатегории и странице
            routes.MapRoute(null,
                "{category}/{subcategory}/{page}",
                new { controller = "Product", action = "List" },
                new { page = @"\d+" }
            );

            // Маршрут для отображения деталей продукта
            routes.MapRoute(null,
                "{category}/{name}/{color}",
                new { controller = "Product", action = "Details" }
            );

            // Общий маршрут по умолчанию для контроллера и действия
            routes.MapRoute(null, "{controller}/{action}");
        }
    }

    //public class RouteConfig
    //{
    //    public static void RegisterRoutes(RouteCollection routes)
    //    {
    //        routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

    //        // Маршрут для главной страницы
    //        routes.MapRoute(
    //            name: "Home",
    //            url: "",
    //            defaults: new { controller = "Product", action = "List", category = (string)null, subcategory = (string)null, page = 1 }
    //        );

    //        // Маршрут для отображения продуктов по категории и странице
    //        routes.MapRoute(
    //            name: "Category",
    //            url: "{category}/{page}",
    //            defaults: new { controller = "Product", action = "List", page = 1 }
    //        );

    //        // Маршрут для отображения продуктов по категории, подкатегории и странице
    //        routes.MapRoute(
    //            name: "CategoryWithSubcategory",
    //            url: "{category}/{subcategory}/{page}",
    //            defaults: new { controller = "Product", action = "List", page = 1 }
    //        );



    //        // Маршрут для отображения деталей продукта
    //        routes.MapRoute(
    //            name: "ProductDetails",
    //            url: "{category}/{name}/{color}",
    //            defaults: new { controller = "Product", action = "Details" }
    //        );

    //        // Общий маршрут по умолчанию для контроллера и действия
    //        routes.MapRoute(
    //            name: "Default",
    //            url: "{controller}/{action}/{id}",
    //            defaults: new { id = UrlParameter.Optional }
    //        );
    //    }
    //}
}
