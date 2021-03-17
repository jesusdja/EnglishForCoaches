using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EnglishForCoachesWeb.Database.Ejercicios
{
    public class CategoriaVocabulario
    {
        public int CategoriaVocabularioId { get; set; }
        public string  Descripcion { get; set; }
        

        public string Descripcion_en { get; set; }
    }
}