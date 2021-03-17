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
    public class TestIndexViewModel
    {
        public Bloque bloque { get; set; }

        public List<Test> tests { get; set; }
        public List<int> mistakes { get; set; }
    }

    public class PreguntaTest
    {
        
        public string UrlImagen { get; set; }
        public string Enunciado { get; set; }

        public string Respuesta1 { get; set; }
        public string Respuesta2 { get; set; }
        public string Respuesta3 { get; set; }
        public string Respuesta4 { get; set; }
        public string Explicacion { get; set; }
    }

    public class ResultadoTest
    {
        public bool Correcto { get; set; }

        public string Explicacion { get; set; }
    }
}
