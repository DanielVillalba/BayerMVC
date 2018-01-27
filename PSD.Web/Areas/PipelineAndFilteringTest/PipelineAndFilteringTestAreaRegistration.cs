using System.Web.Mvc;

namespace PSD.Web.Areas.PipelineAndFilteringTest
{
    public class PipelineAndFilteringTestAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "PipelineAndFilteringTest";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "PipelineAndFilteringTest_default",
                "PipelineAndFilteringTest/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}