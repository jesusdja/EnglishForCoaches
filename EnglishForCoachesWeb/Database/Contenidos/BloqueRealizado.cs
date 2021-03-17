using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EnglishForCoachesWeb.Database.Ejercicios
{
    public class BloqueRealizado
    {
        public int BloqueRealizadoId { get; set; }
        public int BloqueId { get; set; }
        public virtual Bloque Bloque { get; set; }


        [Display(Name = "Fecha realizado")]
        public DateTime FechaRealizado { get; set; }
        public string AlumnoId { get; set; }

    }
}