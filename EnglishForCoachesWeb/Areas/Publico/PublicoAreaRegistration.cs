using System.Web.Mvc;

namespace EnglishForCoachesWeb.Areas.Publico
{
    public class PublicoAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Publico";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            //o artículo: englishforcoaches.com /{ categoría}/{ titulo}
            //o categoría: englishforcoaches.com /{ categoría}
            //o etiqueta: englishforcoaches.com / tag /{ etiqueta}
            //o etiqueta: englishforcoaches.com / search /{ texto}


            context.MapRoute(
                "Publico_about",
                "about",
                new { controller = "Articulos", action = "About" },
                new[] { "EnglishForCoachesWeb.Areas.Publico.Controllers" }
            );

            context.MapRoute(
                "Publico_tag",
                "tag/{id}",
                new { controller = "Articulos", action = "Tag" },
                new[] { "EnglishForCoachesWeb.Areas.Publico.Controllers" }
            );



            context.MapRoute(
                "Publico_buscar",
                "search/{id}",
                new { controller = "Articulos", action = "Buscar" },
                new[] { "EnglishForCoachesWeb.Areas.Publico.Controllers" }
            );


            context.MapRoute(
                "Publico_categoria",
                "{categoriaUrl}",
                new { controller = "Articulos", action = "Categoria"},
                new[] { "EnglishForCoachesWeb.Areas.Publico.Controllers" }
            );

          

            context.MapRoute(
                "Publico_articulo",
                "{categoriaUrl}/{articuloUrl}",
                new { controller = "Articulos", action = "Articulo" },
                new[] { "EnglishForCoachesWeb.Areas.Publico.Controllers" }
            );

            context.MapRoute(
                "Publico_default",
                "Publico/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "EnglishForCoachesWeb.Areas.Publico.Controllers" }
            );
            context.MapRoute(
                "Publico_index",
                "",
                new {controller= "Articulos", action = "Index" },
                new[] { "EnglishForCoachesWeb.Areas.Publico.Controllers" }
            );
        }
    }
}