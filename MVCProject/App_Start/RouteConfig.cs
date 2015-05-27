using System.Web.Mvc;
using System.Web.Routing;
using MVCProject.Models;

namespace MVCProject
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
            name: "ArticleRoute",
            url: "{controller}/{action}/{id}",
            defaults: new { controller = "Article", action = "Index", pageSize = 5, pageNumber = 1, id = UrlParameter.Optional });

            routes.MapRoute(
            name: "HomeRoute",
            url: "{controller}/{action}/{id}",
            defaults: new { controller = "Home", action = "ArticleListing", id = UrlParameter.Optional });
        }
    }
}