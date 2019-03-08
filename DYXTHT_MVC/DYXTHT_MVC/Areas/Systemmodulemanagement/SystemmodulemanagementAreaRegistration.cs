using System.Web.Mvc;

namespace DYXTHT_MVC.Areas.Systemmodulemanagement
{
    public class SystemmodulemanagementAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Systemmodulemanagement";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Systemmodulemanagement_default",
                "Systemmodulemanagement/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}