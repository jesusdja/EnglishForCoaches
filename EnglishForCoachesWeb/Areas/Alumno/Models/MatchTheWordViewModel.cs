using EnglishForCoachesWeb.Database;
using EnglishForCoachesWeb.Database.Ejercicios;
using EnglishForCoachesWeb.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace EnglishForCoachesWeb.Areas.Alumno.Models
{
    public class MatchTheWordIndexViewModel
    {
        public Bloque bloque { get; set; }

        public List<MatchTheWord> MatchTheWords { get; set; }
        public List<string> Preguntas { get; set; }
        public List<string> Respuestas { get; set; }
    }

    public class MatchTheWordResultado
    {
        public bool[] Correctas { get; set; }
        
    }
}