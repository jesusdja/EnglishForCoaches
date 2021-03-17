using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnglishForCoachesWeb.Database.Ejercicios
{
    public class SubTemaVideo
    {
        public int SubTemaVideoId { get; set; }
        [Required]
        public string  Titulo { get; set; }

            [AllowHtml]
        public string UrlVideo { get; set; }



        [Display(Name = "SubTema")]
        public int SubTemaId { get; set; }
        public virtual SubTema SubTema { get; set; }
        
    }
}