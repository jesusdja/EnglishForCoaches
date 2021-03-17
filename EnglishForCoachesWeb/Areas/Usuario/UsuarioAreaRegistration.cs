using System.Web.Mvc;

namespace EnglishForCoachesWeb.Areas.Usuario
{
    public class UsuarioAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Usuario";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
              "Usuario_Home_index",
              "Usuario/Home/Index",
              new { area = "Usuario", controller = "Home", action = "Index" },
              new[] { "EnglishForCoachesWeb.Areas.Usuario.Controllers" }
          );
            context.MapRoute(
              "Usuario_Home_index2",
              "Usuario/Home/",
              new { area = "Usuario", controller = "Home", action = "Index" },
              new[] { "EnglishForCoachesWeb.Areas.Usuario.Controllers" }
          );
            context.MapRoute(
                "Usuario_default",
                "Usuario/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );


            
        }
    }
}