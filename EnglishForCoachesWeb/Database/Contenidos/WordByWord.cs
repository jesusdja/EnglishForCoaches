using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnglishForCoachesWeb.Database.Ejercicios
{
    public class WordByWord: ContenidoBase
    {
        [Required]
        [Display(Name = "Enunciado")]
        public string Enunciado { get; set; }
        
    }
}