using EnglishForCoachesWeb.Database;
using EnglishForCoachesWeb.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnglishForCoachesWeb.Areas.Admin.Models
{

    public class ArticuloViewModel
    {
        public Articulo Articulo { get; set; }

        public SelectList CategoriaSelectList { get; set; }

        [Display(Name = "Tags")]
        public string Tags { get; set; }

        [Display(Name = "Cambiar imagen")]
        [MaxFileSize(3 * 1024 * 1024, ErrorMessage = "El tamaño máximo permitido es de {0} bytes")]
        [MinImageSizeAttribute(1200, 960, ErrorMessage = "El tamaño mínimo de la imagen es de {0}x{1} pixels")]
        public virtual HttpPostedFileBase ImagenPreguntaFile { get; set; }

        public void InicializarDesplegables()
        {
            AuthContext db = new AuthContext();

            CategoriaSelectList = new SelectList(db.Categorias, "CategoriaId", "Descripcion");
        }
    }

    public class ArticuloCreateViewModel : ArticuloViewModel
    {
        [RequiredLocalized]
        public override HttpPostedFileBase ImagenPreguntaFile { get; set; }

        public void Inicializar()
        {
            Articulo = new Articulo();
            Articulo.FechaPublicacion = DateTime.Now;
            InicializarDesplegables();
        }
    }

    public class ArticuloEditViewModel : ArticuloViewModel
    {
        public override HttpPostedFileBase ImagenPreguntaFile { get; set; }

        public void Inicializar(Articulo articulo)
        {
            InicializarDesplegables();
            Articulo = articulo;
            string tags = "";
            foreach (var tag in articulo.Tags)
            {
                tags += tag.Descripcion + ",";
            }
            if (tags.EndsWith(","))
            {
                tags = tags.Remove(tags.Length - 1);
            }
            Tags = tags;

        }
    }
}