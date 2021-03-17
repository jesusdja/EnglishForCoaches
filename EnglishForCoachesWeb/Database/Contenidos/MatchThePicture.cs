using EnglishForCoachesWeb.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnglishForCoachesWeb.Database.Ejercicios
{
    public class MatchThePicture : ContenidoBase
    {
        [Display(Name = "Imagen")]
        public string UrlImagen { get; set; }


        [Required]
        [Display(Name = "Texto")]
        [MaxLength(40)]
        public string PalabraImagen { get; set; }
        
    }
}