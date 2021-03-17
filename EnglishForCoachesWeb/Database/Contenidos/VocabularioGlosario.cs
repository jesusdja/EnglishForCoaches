using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EnglishForCoachesWeb.Database.Ejercicios
{
    public class VocabularioGlosario
    {
        public int VocabularioGlosarioId { get; set; }
        public int VocabularioId { get; set; }
        public virtual Vocabulario Vocabulario { get; set; }


        [Display(Name = "Fecha añadido")]
        public DateTime Fecha { get; set; }


        public string AlumnoId { get; set; }

    }
}