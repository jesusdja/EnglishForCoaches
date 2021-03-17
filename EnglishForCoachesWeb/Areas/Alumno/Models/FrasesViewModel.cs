using EnglishForCoachesWeb.Database.Ejercicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnglishForCoachesWeb.Areas.Alumno.Models
{

    
    public class FrasesIndexViewModel
    {
        public List<Frase> frases { get; set; }
        public SubTema Subtema { get; set; }
        public Gramatica Gramatica { get; set; }

        public List<int> glosario { get; set; }
    }

}