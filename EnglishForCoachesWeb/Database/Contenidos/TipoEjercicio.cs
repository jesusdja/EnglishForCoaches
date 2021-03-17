using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnglishForCoachesWeb.Database.Ejercicios
{
    public class TipoEjercicio
    {
        public int TipoEjercicioId { get; set; }

        [Display(Name = "TipoEjercicio")]
        public string Descripcion { get; set; }

        public string Controlador { get; set; }
        public string EstiloColor { get; set; }
    }

    public enum TiposDeEjerciciosId
    {
        Test=1,
        Skillwise=2,
        MatchTheWord=3,
        FillTheGap=4,
        WordByWord = 5,
        AudioSentenceExercise=6,
        Crucigrama= 7,
        MatchThePicture = 8,
        TrueFalse = 9,
        BeatTheGoalie=10,
        FillTheBox = 11
    }
}