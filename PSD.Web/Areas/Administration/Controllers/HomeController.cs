using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using PSD.Model;
using PSD.Security;

namespace PSD.Web.Areas.Administration.Controllers
{
    //[Authorization(allowedRoles: Controller.UserRole.SysAdmin + "," + Controller.UserRole.AppAdmin)]
    public class HomeController : PSD.Web.Controllers._BaseWebController
    {
        public HomeController()
            : base("Administration.HomeController")
        {

        }
        public ActionResult Index()            
        {
            return RedirectToHome();
        }
	}
}