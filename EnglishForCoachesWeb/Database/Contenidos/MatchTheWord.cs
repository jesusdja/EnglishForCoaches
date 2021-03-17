using EnglishForCoachesWeb.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnglishForCoachesWeb.Database.Ejercicios
{
    public class MatchTheWord:ContenidoBase
    {
        [Required]
        [Display(Name = "Parte 1")]
        public string Pregunta { get; set; }


        [Required]
        [Display(Name = "Parte 2")]
        public string Respuesta { get; set; }
        
    }
}