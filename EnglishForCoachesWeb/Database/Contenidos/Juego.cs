using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnglishForCoachesWeb.Database.Ejercicios
{
    public class Juego
    {
        public int JuegoId { get; set; }
        [Required]
        public string  Titulo { get; set; }

        [Required]
        [AllowHtml]
        public string Explicacion { get; set; }

    
        public string Fichero { get; set; }



        [Display(Name = "SubTema")]
        public int SubTemaId { get; set; }
        public virtual SubTema SubTema { get; set; }


        [Display(Name = "Categoría")]
        public int CategoriaJuegoId { get; set; }

        public virtual CategoriaJuego CategoriaJuego { get; set; }
    }
}