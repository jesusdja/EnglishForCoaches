using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnglishForCoachesWeb.Database.Ejercicios
{
    public class Extra
    {
        public int ExtraId { get; set; }
        [Required]
        public string  Titulo { get; set; }

        [Required]
        [AllowHtml]
        public string Explicacion { get; set; }
        public int? ExtraExplicacionId { get; set; }
        public virtual ExtraExplicacion ExtraExplicacion { get; set; }

        public string Consejo { get; set; }
        public string Foto { get; set; }
        public string Audio { get; set; }
        public string Fichero { get; set; }

        public string UrlVideo { get; set; }


        [Display(Name = "Categoría")]
        public int CategoriaExtraId { get; set; }
        public virtual CategoriaExtra CategoriaExtra { get; set; }

        public int Orden { get; set; }


        [Display(Name = "SubTema")]
        public Nullable<int> SubTemaId { get; set; }
        public virtual SubTema SubTema { get; set; }
    }
    public class ExtraExplicacion
    {
        public int ExtraExplicacionId { get; set; }
        [Required]
        [AllowHtml]
        public string Explicacion { get; set; }

    }
}