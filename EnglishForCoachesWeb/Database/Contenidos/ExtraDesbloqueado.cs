using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EnglishForCoachesWeb.Database.Ejercicios
{
    public class ExtraDesbloqueado
    {
        public int ExtraDesbloqueadoId { get; set; }
        public int ExtraId { get; set; }
        public virtual Extra Extra { get; set; }
                

        [Display(Name = "Fecha desbloqueo")]
        public DateTime FechaDesbloqueo { get; set; }
        

        public string AlumnoId { get; set; }
        
    }
}