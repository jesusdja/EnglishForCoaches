using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EnglishForCoachesWeb.Database.Ejercicios
{
    public class BloqueDesbloqueado
    {
        public int BloqueDesbloqueadoId { get; set; }
        public int BloqueId { get; set; }
        public virtual Bloque Bloque { get; set; }
                

        [Display(Name = "Fecha desbloqueo")]
        public DateTime FechaDesbloqueo { get; set; }
        

        public string AlumnoId { get; set; }
        
    }
}