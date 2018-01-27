using System.Web.Mvc;

namespace PSD.Web.Areas.BenefitPrograms
{
    public class BenefitProgramsAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "BenefitPrograms";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "BenefitPrograms_default",
                "BenefitPrograms/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}