using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnglishForCoachesWeb.Database.Ejercicios
{
    public class Crucigrama : ContenidoBase
    {
        [Required]
        [Display(Name = "Enunciado")]
        public string Enunciado { get; set; }
        public virtual ICollection<CasillaCrucigrama> CasillaCrucigramas { get; set; }

        [Required]
        public string Horizontales { get; set; }
        [Required]
        public string Verticales { get; set; }


       
    }
}