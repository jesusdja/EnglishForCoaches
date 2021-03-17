using EnglishForCoachesWeb.Database.Ejercicios;
using EnglishForCoachesWeb.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnglishForCoachesWeb.Database.JuegoOnline
{
    public class JuegoOnline
    {
        public int JuegoOnlineId { get; set; }

        [RequiredLocalized]
        [Display(Name = "Descripcion")]
        public string Descripcion { get; set; }

        [Display(Name = "Explicación")]
        public string Explicacion { get; set; }


        [Display(Name = "SubTema")]
        public int SubTemaId { get; set; }
        public virtual SubTema SubTema { get; set; }
        
        [Display(Name = "TipoJuegoOnline")]
        public int TipoJuegoOnlineId { get; set; }
        public virtual TipoJuegoOnline TipoJuegoOnline { get; set; }

        
        public virtual ICollection<BeatTheGoalie> BeatTheGoalies { get; set; }
        public virtual ICollection<MemoryGame> MemoryGames { get; set; }
        public virtual ICollection<SopaLetras> SopaLetras { get; set; }
        public virtual ICollection<Ahorcado> Ahorcado { get; set; }
    }
}