using System.Web.Mvc;

namespace PSD.Web.Areas.BenefitProgramsManagement
{
    public class BenefitProgramsManagementAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "BenefitProgramsManagement";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "BenefitProgramsManagement_default",
                "BenefitProgramsManagement/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}