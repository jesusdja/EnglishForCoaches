using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnglishForCoachesWeb.Database.Ejercicios
{
    public class TrueFalse:ContenidoBase
    {

        [Display(Name = "Enunciado")]
        public string Enunciado { get; set; }


        [Display(Name = "Imagen pregunta")]
        public string UrlImagen { get; set; }

        [Display(Name = "Fichero audio")]
        public string Audio { get; set; }

        [Required]
        [Display(Name = "RespuestaCorrecta")]
        public bool RespuestaCorrecta { get; set; }

        [Required]
        [Display(Name = "Explicacion")]
        public string Explicacion { get; set; }



        [Display(Name = "Pregunta de examen")]
        public bool? PreguntaExamen { get; set; }
    }
}