using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnglishForCoachesWeb.Database
{
    public class Categoria
    {
        public int CategoriaId { get; set; }
        [Display(Name = "Categoría")]
        public string Descripcion { get; set; }

        public string DescripcionSeo { get; set; }
    }
}