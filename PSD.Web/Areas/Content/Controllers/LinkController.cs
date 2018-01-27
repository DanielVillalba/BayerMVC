using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using PSD.Model;
using PSD.Security;

namespace PSD.Web.Areas.Content.Controllers
{
    [Authorization(allowedRoles: Controller.UserRole.All)]
    public class LinkController : PSD.Web.Controllers._BaseWebController
    {
        private Controller.Content.LinkController controller;
        public LinkController()
            : base("LinkController")
        {
            controller = new PSD.Controller.Content.LinkController(Configurations);
        }
        public PartialViewResult RetrieveAllPartial()
        {
            List<ContentLink> auxList = controller.RetrieveAll();
            if (controller.ResultManager.IsCorrect)
            {
                return PartialView(auxList);
            }

            NotifyUser(resultManager: controller.ResultManager);
            return null;
        }        
    }
}