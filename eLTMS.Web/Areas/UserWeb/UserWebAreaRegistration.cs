using System.Web.Mvc;

namespace eLTMS.Web.Areas.UserWeb
{
    public class UserWebAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "UserWeb";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "UserWeb_default",
                "UserWeb/{controller}/{action}/{id}",
                new { controller = "HomePage", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "eLTMS.Web.Areas.UserWeb.Controllers" }
            );
        }
    }
}