using EnglishForCoachesWeb.Areas.Admin.Models;
using EnglishForCoachesWeb.Database;
using EnglishForCoachesWeb.Database.Ejercicios;
using EnglishForCoachesWeb.Database.Varios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EnglishForCoachesWeb.Areas.Alumno.Models
{  
    public class PreguntaMistake
    {
        public Bloque bloque { get; set; }
        public string enunciado { get; set; }
        public int bloqueId { get; set; }
        public int preguntaId { get; set; }

        public string tema { get; set; }
        public string subtema { get; set; }
        public string area { get; set; }

        public string controlador { get; set; }
        
    }


    public class MistakesIndexViewModel : ViewModelPaginado
    {

        public List<Mistake> mistakes { get; set; }
        

        public List<PreguntaMistake> preguntas { get; set; }
    }

    public class MistakesViewViewModel
    {
        public Mistake Mistake { get; set; }

    }
    
}