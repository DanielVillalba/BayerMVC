using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PSD.Web.Areas.ContentManagement.Controllers
{
    public class DirectoryController : PSD.Web.Controllers._BaseWebController
    {
        public DirectoryController()
            : base("DirectoryController")
        {
            //controller = new Controller.DistributorController(Configurations);
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Detail(int Id)
        {
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult Edit(int Id)
        {
            return View();
        }
        public ActionResult Delete(int Id)
        {
            return RedirectToAction("index");
        }
	}
}