using EnglishForCoachesWeb.Database.Ejercicios;
using System.Collections.Generic;

namespace EnglishForCoachesWeb.Areas.Alumno.Models
{
    public class AudioSentenceExerciseIndexViewModel
    {
        public Bloque bloque { get; set; }

        public List<AudioSentenceExercise> audioSentence { get; set; }
        public List<int> mistakes { get; set; }
    }

    public class PreguntaAudioSentenceExercise
    {
        public string Enunciado { get; set; }
        public string Audio { get; set; }
        public string Explicacion { get; set; }
    }

    public class ResultadoAudioSentenceExercise
    {
        public bool Correcto { get; set; }

        public List<bool> correctas { get; set; }
    }
}