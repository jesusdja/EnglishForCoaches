using EnglishForCoachesWeb.Database.Ejercicios;
using EnglishForCoachesWeb.Database.JuegoOnline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnglishForCoachesWeb.Areas.Alumno.Models
{  

    public class JuegoOnlinesIndexViewModel
    {
        public List<JuegoOnline> listadoJuegoOnlines{ get; set; }
        public List<int> realizados { get; set; }
        public List<int> conMistakes { get; set; }

        public SubTema Subtema { get; set; }

        
        public List<int> juegoOnlinesDesbloqueados { get; set; }
    }
    
}