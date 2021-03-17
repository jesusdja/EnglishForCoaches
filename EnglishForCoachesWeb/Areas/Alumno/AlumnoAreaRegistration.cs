using System.Web.Mvc;

namespace EnglishForCoachesWeb.Areas.Alumno
{
    public class AlumnoAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Alumno";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {


            context.MapRoute(
              "Cliente_Alumno_Crucigramas_index",
              "{cliente}/Alumno/{controller}/Index",
              new { area = "Alumno", action = "Index" },
              new[] { "EnglishForCoachesWeb.Areas.Alumno.Controllers" }
          );

            context.MapRoute(
                "Cliente_Alumno_default",
                "{cliente}/Alumno/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute(
              "Alumno_Home_index",
              "Alumno/{controller}/Index",
              new {  area = "Alumno", action = "Index" },
              new[] { "EnglishForCoachesWeb.Areas.Alumno.Controllers" }
          );

            context.MapRoute(
                "Alumno_default",
                "Alumno/{controller}/{action}/{id}",
                new {  action = "Index", id = UrlParameter.Optional }
            );



        }
    }
}