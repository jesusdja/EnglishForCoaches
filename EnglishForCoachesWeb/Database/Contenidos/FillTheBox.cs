using EnglishForCoachesWeb.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnglishForCoachesWeb.Database.Ejercicios
{
    public class FillTheBox : ContenidoBase
    {
        [Display(Name = "Imagen")]
        public string UrlImagen { get; set; }

        [Display(Name = "Titulo")]
        public string Titulo { get; set; }

        [Display(Name = "Enunciado")]
        public string Enunciado { get; set; }

        [Required]
        [RespuestasFillTheGap("Enunciado")]
        [Display(Name = "Respuestas")]
        public string Respuestas { get; set; }

        [Display(Name = "Respuestas Incorrectas")]
        public string RespuestasIncorrectas { get; set; }


        [Required]
        [Display(Name = "Explicacion")]
        public string Explicacion { get; set; }


        [Display(Name = "Fichero de audio")]
        public string FicheroAudio { get; set; }
    }
}