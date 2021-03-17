using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EnglishForCoachesWeb.Database.JuegoOnline
{
    public class JuegoOnlineRealizado
    {
        public int JuegoOnlineRealizadoId { get; set; }
        public int JuegoOnlineId { get; set; }
        public virtual JuegoOnline JuegoOnline { get; set; }


        [Display(Name = "Fecha realizado")]
        public DateTime FechaRealizado { get; set; }
        public string AlumnoId { get; set; }

    }
}