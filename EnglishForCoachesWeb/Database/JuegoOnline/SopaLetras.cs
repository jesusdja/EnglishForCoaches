using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnglishForCoachesWeb.Database.JuegoOnline
{
    public class SopaLetras : JuegoOnlineBase
    {
        [Required]
        [Display(Name = "Enunciado")]
        public string Enunciado { get; set; }
        public virtual ICollection<CasillaSopaLetras> CasillaSopaLetras { get; set; }
        public virtual ICollection<ComentarioSopaLetras> ComentarioSopaLetras { get; set; }
        public virtual ICollection<VocabularioSopaLetras> VocabularioSopaLetras { get; set; }




    }
}