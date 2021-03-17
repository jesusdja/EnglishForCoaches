using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnglishForCoachesWeb.Database.Ejercicios
{
    public class SubTema
    {
        public int SubTemaId { get; set; }

        [Display(Name = "SubTema")]
        public string Descripcion { get; set; }


        [Display(Name = "Tema")]
        public int TemaId { get; set; }
        public virtual Tema Tema { get; set; }

        public int Orden { get; set; }
    }
}