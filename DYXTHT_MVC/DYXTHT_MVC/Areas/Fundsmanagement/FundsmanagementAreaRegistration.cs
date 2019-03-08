using System.Web.Mvc;

namespace DYXTHT_MVC.Areas.Fundsmanagement
{
    public class FundsmanagementAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Fundsmanagement";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Fundsmanagement_default",
                "Fundsmanagement/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}