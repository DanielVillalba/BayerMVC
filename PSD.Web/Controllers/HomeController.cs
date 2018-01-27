using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using PSD.Security;

namespace PSD.Web.Controllers
{
    public class HomeController : _BaseWebController
    {
        public HomeController()
            : base("HomeController")
        {

        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }
        
    }
}