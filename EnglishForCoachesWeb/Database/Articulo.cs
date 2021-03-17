using EnglishForCoachesWeb.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnglishForCoachesWeb.Database
{
    public class Articulo
    {
        public int ArticuloId { get; set; }
        

        [Display(Name = "Categoria")]
        public int CategoriaId { get; set; }
        public virtual Categoria Categoria { get; set; }


        [Display(Name = "Fecha alta")]
        public Nullable<DateTime> FechaAlta { get; set; }

        [Display(Name = "Fecha publicación")]
        [RequiredLocalized]
        public DateTime FechaPublicacion { get; set; }

        [Display(Name = "Publicado")]
        [RequiredLocalized]
        public bool Publicado { get; set; }


        [Display(Name = "Título")]
        [RequiredLocalized]
        public string Titulo { get; set; }


        public string TituloSeo { get; set; }

        [AllowHtml]
        [RequiredLocalized]
        [Display(Name = "Cuerpo")]
        public string Cuerpo { get; set; }

        public string CuerpoResumen { get; set; }



        public virtual ICollection<Comentario> Comentarios { get; set; }


        public virtual ICollection<Tag> Tags { get; set; }

    }
}

