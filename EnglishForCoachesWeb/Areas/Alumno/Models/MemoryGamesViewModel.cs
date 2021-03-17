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
    public class CasillaMemoryGame{
        public string Dato { get; set; }
        public string Tipo { get; set; }
        public string Pareja { get; set; }
    }

    public class MemoryGameIndexViewModel
    {
        public JuegoOnline juegoOnline { get; set; }

        public List<MemoryGame> MemoryGames { get; set; }
        public List<CasillaMemoryGame> Casillas { get; set; }
        
    }


    public class ResultadoMemoryGame
    {
        public bool[] Correctas { get; set; }
    }
}
