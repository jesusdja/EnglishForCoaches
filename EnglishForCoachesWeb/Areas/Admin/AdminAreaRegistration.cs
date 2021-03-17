using System.Web.Mvc;

namespace EnglishForCoachesWeb.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
          
            context.MapRoute(
              "Admin_ErroresExamens_index",
              "Admin/{controller}/Index",
              new { area = "Admin",  action = "Index" },
              new[] { "EnglishForCoachesWeb.Areas.Admin.Controllers" }
          );
            
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "EnglishForCoachesWeb.Areas.Admin.Controllers" }
            );
        }
    }
}