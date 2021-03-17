using EnglishForCoachesWeb.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnglishForCoachesWeb.Database.Ejercicios
{
    public class Skillwise:ContenidoBase
    {
        [Required]        
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
        [Range(1, 4)]
        public int RespuestaCorrecta { get; set; }

        [Required]
        [CamposConectados ("Respuesta1")]
        [Display(Name = "Explicacion1")]
        public string Explicacion1 { get; set; }

        [Required]
        [CamposConectados("Respuesta2")]
        [Display(Name = "Explicacion2")]
        public string Explicacion2 { get; set; }

        [CamposConectados("Respuesta3")]
        [Display(Name = "Explicacion3")]
        public string Explicacion3 { get; set; }

        [CamposConectados("Respuesta4")]
        [Display(Name = "Explicacion4")]
        public string Explicacion4 { get; set; }
    }
}