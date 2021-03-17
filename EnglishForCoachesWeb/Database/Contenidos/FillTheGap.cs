using EnglishForCoachesWeb.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnglishForCoachesWeb.Database.Ejercicios
{
    public class FillTheGap : ContenidoBase
    {        
        [Display(Name = "Enunciado")]
        public string Enunciado { get; set; }

        [Required]
        [RespuestasFillTheGap("Enunciado")]
        [Display(Name = "Respuestas")]
        public string Respuestas { get; set; }

        
        [Display(Name = "Explicacion")]
        public string Explicacion { get; set; }

        [Display(Name = "Pregunta de examen")]
        public bool? PreguntaExamen { get; set; }
    }
}