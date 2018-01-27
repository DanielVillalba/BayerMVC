using PSD.Model;
using PSD.Security;
using PSD.Web.Areas.ContentManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PSD.Web.Areas.ContentManagement.Controllers
{
    [Authorization(allowedRoles: "appadmin")]
    public class ContactPageController : PSD.Web.Controllers._BaseWebController
    {
        private Controller.Content.ContentDataController controller;
        public ContactPageController()
            : base("ContactPageController")
        {
            controller = new Controller.Content.ContentDataController(Configurations);
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ContactPageUpdate()
        {
            var key = Controller.Content.ContentTypeKey.ContactPageContent;
            ContentData itemToEdit = controller.GetContentDataByKey(key);
            UpdateHtmlContentViewModel model = new UpdateHtmlContentViewModel()
            {
                Content = HttpUtility.HtmlDecode(itemToEdit != null ? itemToEdit.Value : "<p><br></p>")
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ContactPageUpdate(UpdateHtmlContentViewModel model)
        {
            var jumbotronTextKey = Controller.Content.ContentTypeKey.ContactPageContent;

            string encodedContent = HttpUtility.HtmlEncode(model.Content);

            if (controller.Update(jumbotronTextKey, encodedContent) || controller.ResultManager.IsCorrect)
            {
                NotifyUser(messageOk: "Contenido texto para pagina de contacto editado correctamente");
                return RedirectToAction("Index");
            }

            NotifyUser(resultManager: controller.ResultManager);
            return View(model);
        }
    }
}