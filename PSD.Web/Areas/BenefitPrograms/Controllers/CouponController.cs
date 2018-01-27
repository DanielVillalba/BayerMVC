using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using PSD.Model;
using PSD.Security;

namespace PSD.Web.Areas.BenefitPrograms.Controllers
{
    public class CouponController : PSD.Web.Controllers._BaseWebController
    {
        private Controller.BenefitPrograms.Coupon.CouponController controller;
        public CouponController()
            : base("CouponController")
        {
            controller = new Controller.BenefitPrograms.Coupon.CouponController(Configurations);
        }
        public ActionResult Index()
        {
            return View();
        }
    }
}