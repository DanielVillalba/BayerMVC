using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSD.Web.Areas.BenefitProgramsManagement.Models
{
    public class CouponConfigurationsModel
    {
        public bool IsOpen { get; set; }
        public bool S1IsOpen { get; set; }
        public bool S2IsOpen { get; set; }
        public bool IsCalculated { get; set; }
        public bool S1IsCalculated { get; set; }
        public bool S2IsCalculated { get; set; }
    }
}