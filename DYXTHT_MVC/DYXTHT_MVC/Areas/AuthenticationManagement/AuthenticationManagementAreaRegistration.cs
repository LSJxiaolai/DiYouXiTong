using System.Web.Mvc;

namespace DYXTHT_MVC.Areas.AuthenticationManagement
{
    public class AuthenticationManagementAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "AuthenticationManagement";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "AuthenticationManagement_default",
                "AuthenticationManagement/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}