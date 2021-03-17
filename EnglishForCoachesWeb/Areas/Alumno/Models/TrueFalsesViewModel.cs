using EnglishForCoachesWeb.Database;
using EnglishForCoachesWeb.Database.Ejercicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using System.Data.Entity;

namespace EnglishForCoachesWeb.Areas.Alumno.Models
{
    public class TrueFalseIndexViewModel
    {
        public Bloque bloque { get; set; }

        public List<TrueFalse> TrueFalses { get; set; }
        public List<int> mistakes { get; set; }
    }

    public class PreguntaTrueFalse
    {
        
        public string Audio { get; set; }
        public string UrlImagen { get; set; }
        public string Enunciado { get; set; }
        public string Explicacion { get; set; }

    }

    public class ResultadoTrueFalse
    {
        public bool Correcto { get; set; }

        public string Explicacion { get; set; }
    }
}
