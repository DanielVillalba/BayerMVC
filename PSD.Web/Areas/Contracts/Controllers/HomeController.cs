using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using PSD.Model;
using PSD.Security;

namespace PSD.Web.Areas.Contracts.Controllers
{
    [Authorization(allowedRoles: "appadmin,employee-manager_operation,employee-rtv_operation")]
    public class HomeController : PSD.Web.Controllers._BaseWebController
    {
        private Controller.DistributorController controller;
        public HomeController()
            : base("HomeController")
        {
            controller = new Controller.DistributorController(Configurations);///TODO: check when at lifetime of page to set hosturl, at constructor time the Request Object is null
        }
        public ActionResult Index()
        {
            return RedirectToHome();
        }
	}
}