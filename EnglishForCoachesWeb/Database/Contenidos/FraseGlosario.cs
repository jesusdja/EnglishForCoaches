using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EnglishForCoachesWeb.Database.Ejercicios
{
    public class FraseGlosario
    {
        public int FraseGlosarioId { get; set; }
        public int FraseId { get; set; }
        public virtual Frase Frase { get; set; }


        [Display(Name = "Fecha añadido")]
        public DateTime Fecha { get; set; }


        public string AlumnoId { get; set; }

    }
}