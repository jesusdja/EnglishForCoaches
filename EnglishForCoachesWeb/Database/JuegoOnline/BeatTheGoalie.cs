using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnglishForCoachesWeb.Database.JuegoOnline
{
    public class BeatTheGoalie: JuegoOnlineBase
    {

        [Display(Name = "Fichero de audio")]
        public string FicheroAudio { get; set; }

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
    }
}