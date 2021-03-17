using EnglishForCoachesWeb.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnglishForCoachesWeb.Database.Ejercicios
{
    public class Gramatica
    {
        public int GramaticaId { get; set; }

        [Display(Name = "Titulo")]
        [RequiredLocalized]
        public string Titulo { get; set; }

        [AllowHtml]
        [Display(Name = "Cuerpo")]
        public string Cuerpo { get; set; }

        public int? GramaticaCuerpoId { get; set; }
        public virtual GramaticaCuerpo GramaticaCuerpo { get; set; }

        [Display(Name = "SubTema")]
        public int SubTemaId { get; set; }
        public virtual SubTema SubTema { get; set; }

        public int Orden { get; set; }

        public virtual ICollection<Vocabulario> Vocabularios { get; set; }
    }



    public class GramaticaCuerpo
    {
        public int GramaticaCuerpoId { get; set; }
        [AllowHtml]
        [RequiredLocalized]
        [Display(Name = "Cuerpo")]
        public string Cuerpo { get; set; }
    }
}