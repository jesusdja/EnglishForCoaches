using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnglishForCoachesWeb.Database.Ejercicios
{
    public class Area
    {
        public int AreaId { get; set; }

        [Display(Name = "Area")]
        public string Descripcion { get; set; }

        public int? Orden { get; set; }
        public string Icono { get; set; }
        public string EstiloColor { get; set; }

    }



    public enum AreasId
    {
        Learning = 1,
        Practising = 2,
        Improving = 3,
        Training = 4,
        Listening = 5,
        Answering = 6,
        Playing = 7,
        Writing = 8,
        Examen = 9
    }
}