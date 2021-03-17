using EnglishForCoachesWeb.Database.Ejercicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnglishForCoachesWeb.Areas.Alumno.Models
{  

    public class BloquesIndexViewModel
    {
        public List<Bloque> listadoBloques{ get; set; }
        public List<int> realizados { get; set; }
        public List<int> conMistakes { get; set; }

        public SubTema Subtema { get; set; }
        public Area Area { get; set; }

        
        public List<int> bloquesDesbloqueados { get; set; }
    }
    
}