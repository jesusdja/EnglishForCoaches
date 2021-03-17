using EnglishForCoachesWeb.Database.Ejercicios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnglishForCoachesWeb.Database.JuegoOnline
{
    public class JuegoOnlineBase
    {
        public int Id { get; set; }

        [Display(Name = "Descripcion")]
        public string Descripcion { get; set; }


        [Display(Name = "SubTema")]
        public int SubTemaId { get; set; }
        public virtual SubTema SubTema { get; set; }

    
        
        [Display(Name = "TipoJuegoOnline")]
        public int TipoJuegoOnlineId { get; set; }
        public virtual TipoJuegoOnline TipoJuegoOnline { get; set; }


        [Display(Name = "JuegoOnline")]
        public int JuegoOnlineId { get; set; }
        public virtual JuegoOnline JuegoOnline { get; set; }
    }
}