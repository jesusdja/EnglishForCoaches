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
using EnglishForCoachesWeb.Database.JuegoOnline;

namespace EnglishForCoachesWeb.Areas.Alumno.Models
{
    public class BeatTheGoalieIndexViewModel
    {
        public JuegoOnline juegoOnline { get; set; }

        public List<BeatTheGoalie> BeatTheGoalies { get; set; }
        public List<int> mistakes { get; set; }
    }

    public class PreguntaBeatTheGoalie
    {        
        public string Enunciado { get; set; }
        public string Audio { get; set; }

        public string Respuesta1 { get; set; }
        public string Respuesta2 { get; set; }
        public string Respuesta3 { get; set; }
        public string Respuesta4 { get; set; }
    }

    public class ResultadoBeatTheGoalie
    {
        public bool Correcto { get; set; }

        public string Explicacion { get; set; }
    }
}
