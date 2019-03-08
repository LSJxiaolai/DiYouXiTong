using System.Web.Mvc;

namespace DYXTHT_MVC.Areas.PopularizeManage
{
    public class PopularizeManageAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "PopularizeManage";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "PopularizeManage_default",
                "PopularizeManage/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}