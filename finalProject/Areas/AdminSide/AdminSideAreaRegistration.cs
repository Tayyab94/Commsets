using System.Web.Mvc;

namespace finalProject.Areas.AdminSide
{
    public class AdminSideAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "AdminSide";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "AdminSide_default",
                "AdminSide/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "finalProject.Areas.AdminSide.Controllers" }
            );
        }
    }
}