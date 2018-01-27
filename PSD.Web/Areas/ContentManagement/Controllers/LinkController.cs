using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using PSD.Model;
using PSD.Security;

namespace PSD.Web.Areas.ContentManagement.Controllers
{
    [Authorization(allowedRoles: Controller.UserRole.AppAdmin)]
    public class LinkController : PSD.Web.Controllers._BaseWebController
    {
        private Controller.Content.LinkController controller;
        public LinkController()
            : base("LinkController")
        {
            controller = new PSD.Controller.Content.LinkController(Configurations);
        }
        public ActionResult Index()
        {
            List<ContentLink> auxList = controller.RetrieveAll();
            if (controller.ResultManager.IsCorrect)
            {
                return View(auxList);
            }

            NotifyUser(resultManager: controller.ResultManager);
            return RedirectToAction("Index");
        }
        public ActionResult Add()
        {
            return View(new ContentLink());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(ContentLink item)
        {
            if (controller.Add(item) || controller.ResultManager.IsCorrect)
            {
                NotifyUser(messageOk: "Link agregado correctamente");
                return RedirectToAction("Index");
            }

            NotifyUser(resultManager: controller.ResultManager);
            return View(item);
        }
        public ActionResult Edit(int Id)
        {
            ContentLink itemToEdit = controller.Retrieve(Id);
            if (controller.ResultManager.IsCorrect)
            {
                return View(itemToEdit);
            }

            NotifyUser(resultManager: controller.ResultManager);
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ContentLink model)
        {
            if (controller.Update(model) || controller.ResultManager.IsCorrect)
            {
                NotifyUser(messageOk: "Link editado correctamente");
                return RedirectToAction("Index");
            }

            NotifyUser(resultManager: controller.ResultManager);
            return View(model);
        }
        public ActionResult Delete(int Id)
        {
            if (controller.Delete(Id) || controller.ResultManager.IsCorrect)
            {
                NotifyUser(messageOk: "Link eliminado correctamente");
                return RedirectToAction("Index");
            }

            NotifyUser(resultManager: controller.ResultManager);
            return RedirectToAction("Index");
        }
    }
}