using EnglishForCoachesWeb.Database.Ejercicios;
using System.Collections.Generic;

namespace EnglishForCoachesWeb.Areas.Alumno.Models
{
    public class FillTheBoxIndexViewModel
    {
        public Bloque bloque { get; set; }

        public List<FillTheBox> fillTheBoxs { get; set; }
        public List<int> mistakes { get; set; }
    }

    public class PreguntaFillTheBox
    {
        public string Titulo { get; set; }
        public string Audio { get; set; }
        public string Imagen { get; set; }

        public string[] Enunciado { get; set; }
        public string[] Respuestas { get; set; }
    }

    public class ResultadoFillTheBox
    {
        public bool Correcto { get; set; }

        public bool[] correctas { get; set; }
        public string Explicacion { get; set; }
    }

}
