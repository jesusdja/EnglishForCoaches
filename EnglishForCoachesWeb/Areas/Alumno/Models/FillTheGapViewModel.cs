using EnglishForCoachesWeb.Database.Ejercicios;
using System.Collections.Generic;

namespace EnglishForCoachesWeb.Areas.Alumno.Models
{
    public class FillTheGapIndexViewModel
    {
        public Bloque bloque { get; set; }

        public List<FillTheGap> fillTheGaps { get; set; }
        public List<int> mistakes { get; set; }
    }

    public class PreguntaFillTheGap
    {
        public string Enunciado { get; set; }

        public string Respuestas { get; set; }
        public string Explicacion { get; set; }
    }

    public class ResultadoFillTheGap
    {

        public string Explicacion { get; set; }
        public bool Correcto { get; set; }
        
        public List<bool> correctas { get; set; }
    }

}
