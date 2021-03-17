using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

using EnglishForCoachesWeb.Validation;

namespace EnglishForCoachesWeb.Database.Ejercicios
{
    public class RespuestaIncorrecta
    {
        public int RespuestaIncorrectaId { get; set; }
        public int ExamenId { get; set; }
    
        public string Pregunta { get; set; }

        public string RespuestaSeleccionada { get; set; }
        public string RespuestaCorrecta { get; set; }

        public string Tipo { get; set; }
        public int? PreguntaId { get; set; }
    }
}