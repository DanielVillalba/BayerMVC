using System.Web.Mvc;

namespace PSD.Web.Areas.ContentManagement
{
    public class ContentManagementAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ContentManagement";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ContentManagemen_News",
                "ContentManagement/{controller}/Detail/{titleId}",
                new { controller = "News", action = "Detail", titleId = UrlParameter.Optional }
            );
            context.MapRoute(
                "ContentManagement_default",
                "ContentManagement/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}