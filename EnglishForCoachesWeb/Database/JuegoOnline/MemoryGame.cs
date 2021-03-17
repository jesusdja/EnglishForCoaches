using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnglishForCoachesWeb.Database.JuegoOnline
{
    public class MemoryGame : JuegoOnlineBase
    {

        [Display(Name = "Imagen")]
        public string UrlImagen { get; set; }


        [Required]
        [Display(Name = "Texto")]
        [MaxLength(24)]
        public string PalabraImagen { get; set; }
    }
}