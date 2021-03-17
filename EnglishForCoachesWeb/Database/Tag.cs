using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnglishForCoachesWeb.Database
{
    public class Tag
    {
        public int TagId { get; set; }
        
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        public string DescripcionSeo { get; set; }

        public virtual ICollection<Articulo> Articulos { get; set; }
    }
}