using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnglishForCoachesWeb.Database.Ejercicios
{
    public class Test:ContenidoBase
    {
        [Display(Name = "Imagen pregunta")]
        public string UrlImagen { get; set; }


        [Display(Name = "Enunciado")]
        public string Enunciado { get; set; }

        [Required]
        [Display(Name = "Respuesta1")]
        public string Respuesta1 { get; set; }

        [Required]
        [Display(Name = "Respuesta2")]
        public string Respuesta2 { get; set; }
        
        [Display(Name = "Respuesta3")]
        public string Respuesta3 { get; set; }
        
        [Display(Name = "Respuesta4")]
        public string Respuesta4 { get; set; }

        [Required]
        [Display(Name = "RespuestaCorrecta")]
        [Range(1,4)]
        public int RespuestaCorrecta { get; set; }

        [Required]
        [Display(Name = "Explicacion")]
        public string Explicacion { get; set; }

        [Display(Name = "Pregunta de examen")]
        public bool? PreguntaExamen { get; set; }
    }
}