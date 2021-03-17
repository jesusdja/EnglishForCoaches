using EnglishForCoachesWeb.Database.Ejercicios;
using EnglishForCoachesWeb.Database.Foro;
using EnglishForCoachesWeb.Database.Varios;
using System.Collections.Generic;

namespace EnglishForCoachesWeb.Areas.Alumno.Models
{
    public class CalendarioIndexViewModel
    {



    }
    public class EventoCalendario
    {
        public string title { get; set; }
        public string description { get; set; }
        public string type { get; set; }
        public string bg { get; set; }
        public string allday { get; set; }
        public string start { get; set; }
        public string end { get; set; }
    }
    public class EventoDia
    {
        public string texto { get; set; }
        public string tipo { get; set; }
        public string hora { get; set; }
    }

    
}
