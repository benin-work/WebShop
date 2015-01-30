using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebShop.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "About what?";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "You know where you could find me...";

            return View();
        }
    }
}