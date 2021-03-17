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
using EnglishForCoachesWeb.Database.JuegoOnline;

namespace EnglishForCoachesWeb.Areas.Alumno.Models
{
    public class AhorcadoIndexViewModel
    {
        public JuegoOnline juegoOnline { get; set; }



        public string MascaraInicial { get; set; }
        public List<Ahorcado> Ahorcados { get; set; }
    }

    public class PreguntaAhorcado
    {
        public string Enunciado { get; set; }
        public string Audio { get; set; }
        public string Imagen { get; set; }
        public string MascaraInicial { get; set; }

        

    }

    public class AhorcadoResultado
    {
        public int[] posiciones { get; set; }
        public bool Correcto { get; set; }
        public bool Finalizado { get; set; }
        public string Mascara { get; set; }

        public string Explicacion { get; set; }
        //public string Comentario { get; set; }
        //public string textoFinalizado { get; set; }

    }

}