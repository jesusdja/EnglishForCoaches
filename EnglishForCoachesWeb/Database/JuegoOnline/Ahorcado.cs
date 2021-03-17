using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnglishForCoachesWeb.Database.JuegoOnline
{
    public class Ahorcado : JuegoOnlineBase
    {
        [Required]
        [Display(Name = "Texto pista")]
        public string Texto { get; set; }


        [Display(Name = "Imagen pista")]
        public string UrlImagen { get; set; }

        [Display(Name = "Fichero audio")]
        public string Audio { get; set; }

        public string Respuesta { get; set; }

        [Required]
        [Display(Name = "Explicacion")]
        public string Explicacion { get; set; }
    }
}