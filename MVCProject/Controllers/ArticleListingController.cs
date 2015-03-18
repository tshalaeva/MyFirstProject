using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCProject.Controllers
{
    public class ArticleListingController : Controller
    {
        //
        // GET: /ArticleListing/

        public ActionResult Index()
        {
            return View();
        }

        public RedirectResult GoBack()
        {
            return Redirect("~/Home/Index");
        }

    }
}
