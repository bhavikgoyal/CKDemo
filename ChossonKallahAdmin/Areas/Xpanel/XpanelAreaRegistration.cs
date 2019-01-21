using System.Web.Mvc;

namespace ChossonKallahAdmin.Areas.Xpanel
{
    public class XpanelAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Xpanel";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Xpanel_default",
                "Xpanel/{controller}/{action}/{id}",
                new { controller = "Security", action = "Login", id = UrlParameter.Optional }
            );
        }
    }
}