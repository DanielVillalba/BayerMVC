using PSD.Model;
using PSD.Security;
using PSD.Web.Areas.ContentManagement.Models;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PSD.Web.Areas.ContentManagement.Controllers
{
    [Authorization(allowedRoles: "appadmin")]
    public class StartPageController : PSD.Web.Controllers._BaseWebController
    {
        private Controller.Content.ContentDataController controller;
        public StartPageController()
            : base("StartPageController")
        {
            controller = new Controller.Content.ContentDataController(Configurations);
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult UpdateJumbotronContent()
        {
            var titleKey = Controller.Content.ContentTypeKey.StartPageTitle;
            ContentData titleToEdit = controller.GetContentDataByKey(titleKey);

            var subtitleKey = Controller.Content.ContentTypeKey.StartPageSubtitle;
            ContentData subtitleToEdit = controller.GetContentDataByKey(subtitleKey);

            var paragraphKey = Controller.Content.ContentTypeKey.StartPageParagraph;
            ContentData pargraphToEdit = controller.GetContentDataByKey(paragraphKey);

            var buttonKey = Controller.Content.ContentTypeKey.StartPageButton;
            ContentData buttonToEdit = controller.GetContentDataByKey(buttonKey);

            UpdateStartPageContentViewModel model = new UpdateStartPageContentViewModel()
            {
                Title = HttpUtility.HtmlDecode(titleToEdit != null ? titleToEdit.Value : "<p><br></p>"),
                Subtitle = HttpUtility.HtmlDecode(subtitleToEdit != null ? subtitleToEdit.Value : "<p><br></p>"),
                Paragraph = HttpUtility.HtmlDecode(pargraphToEdit != null ? pargraphToEdit.Value : "<p><br></p>"),
                Button = HttpUtility.HtmlDecode(buttonToEdit != null ? buttonToEdit.Value : "<p><br></p>")
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateJumbotronContent(UpdateStartPageContentViewModel model)
        {
            var titleKey = Controller.Content.ContentTypeKey.StartPageTitle;
            var subtitleKey = Controller.Content.ContentTypeKey.StartPageSubtitle;
            var paragraphKey = Controller.Content.ContentTypeKey.StartPageParagraph;
            var buttonKey = Controller.Content.ContentTypeKey.StartPageButton;

            string encodedTitle = HttpUtility.HtmlEncode(model.Title);
            string encodedSubtitle = HttpUtility.HtmlEncode(model.Subtitle);
            string encodedParagraph = HttpUtility.HtmlEncode(model.Paragraph);
            string encodedButton = HttpUtility.HtmlEncode(model.Button);

            if (controller.Update(titleKey, encodedTitle) &&
                controller.Update(subtitleKey, encodedSubtitle) &&
                controller.Update(paragraphKey, encodedParagraph) &&
                controller.Update(buttonKey, encodedButton) &&
                controller.ResultManager.IsCorrect)
            {
                NotifyUser(messageOk: "Contenido texto de jumbotron editado correctamente");
                return RedirectToAction("Index");
            }

            NotifyUser(resultManager: controller.ResultManager);
            return View(model);
        }

        [HttpGet]
        public ActionResult UpdateJumbotronBkg()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateJumbotronBkg(HttpPostedFileBase Image)
        {
            var validImageTypes = new string[]  //TODO: Send to webconfig
            {
                    "image/jpeg",
                    "image/pjpeg",
            };

            if (Image != null && Image.ContentLength > 0)
            {
                if (!validImageTypes.Contains(Image.ContentType))
                    NotifyUser(messageError: "La imagen de fondo del Jumbotron no puede ser un formato '" + Image.ContentType + "', por favor intente de nuevo con un formato valido de imagen (jpeg, pjpeg).");
                else
                {
                    string _path = Path.Combine(Server.MapPath(ImageStoragePath), "jumbotron.jpg");
                    Image.SaveAs(_path);
                    NotifyUser(messageOk: "La imagen de fondo del Jumbotron ha sido actualizada correctamente.");
                    return RedirectToAction("Index");
                }
            }
            
            return View();
        }
    }
}