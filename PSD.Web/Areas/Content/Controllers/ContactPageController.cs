using PSD.Web.Areas.Content.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PSD.Web.Areas.Content.Controllers
{
    public class ContactPageController : PSD.Web.Controllers._BaseWebController
    {
        private Controller.Content.ContentDataController controller;
        public ContactPageController()
            : base("ContactPageController")
        {
            controller = new Controller.Content.ContentDataController(Configurations);
        }
        // GET: Content/ContactPage
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Contact()
        {
            ContactPageViewModel model = new ContactPageViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(ContactPageViewModel model)
        {
            if (controller.SendContactMail(model.Name, model.EMail, model.Message) && controller.ResultManager.IsCorrect)
            {
                NotifyUser(resultManager: controller.ResultManager);
                return RedirectToAction("Index");
            }

            NotifyUser(resultManager: controller.ResultManager);
            return View(model);
        }
    }
}