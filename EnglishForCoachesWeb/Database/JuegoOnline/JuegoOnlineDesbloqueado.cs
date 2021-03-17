using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EnglishForCoachesWeb.Database.JuegoOnline
{
    public class JuegoOnlineDesbloqueado
    {
        public int JuegoOnlineDesbloqueadoId { get; set; }
        public int JuegoOnlineId { get; set; }
        public virtual JuegoOnline JuegoOnline { get; set; }
                

        [Display(Name = "Fecha desbloqueo")]
        public DateTime FechaDesbloqueo { get; set; }
        

        public string AlumnoId { get; set; }
        
    }
}