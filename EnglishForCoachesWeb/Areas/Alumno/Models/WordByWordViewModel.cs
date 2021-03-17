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

    public class WordByWordIndexViewModel
    {
        public Bloque bloque { get; set; }

        public List<WordByWord> WordByWords { get; set; }
        public List<int> mistakes { get; set; }
    }


    public class PreguntaWordByWord
    {
        public string[] Enunciado { get; set; }
        
    }
    public class WordByWordResultado
    {
        public bool[] Correctas { get; set; }
        
    }
}