using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PSD.Web.Areas.Content.Controllers
{
    public class DirectoryController : PSD.Web.Controllers._BaseWebController
    {
        //private Controller.DistributorController controller;

        public DirectoryController()
            : base("DirectoryController")
        {
            //controller = new Controller.DistributorController(Configurations);
        }
        public ActionResult Index()
        {
            return View();
        }
	}
}