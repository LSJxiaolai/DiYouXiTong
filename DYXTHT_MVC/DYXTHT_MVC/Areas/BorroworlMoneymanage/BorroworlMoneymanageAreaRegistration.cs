using System.Web.Mvc;

namespace DYXTHT_MVC.Areas.BorroworlMoneymanage
{
    public class BorroworlMoneymanageAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "BorroworlMoneymanage";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "BorroworlMoneymanage_default",
                "BorroworlMoneymanage/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}