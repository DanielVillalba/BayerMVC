using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PSD.Model;

namespace PSD.Controller.BenefitPrograms.Coupon
{
    public class CouponController : _BaseController
    {
        private Controller.BenefitPrograms.Coupon.CouponController controller;
        public CouponController(IAppConfiguration configurations)
            : base("CouponController.", configurations)
        {

        }

    }
}
