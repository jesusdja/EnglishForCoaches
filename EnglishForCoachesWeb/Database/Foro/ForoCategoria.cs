using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EnglishForCoachesWeb.Database.Foro
{
    public class ForoCategoria
    {
        public int ForoCategoriaId { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
  

    }
}