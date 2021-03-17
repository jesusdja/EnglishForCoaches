using EnglishForCoachesWeb.Database.Ejercicios;
using EnglishForCoachesWeb.Database.Foro;
using EnglishForCoachesWeb.Database.Varios;
using System.Collections.Generic;

namespace EnglishForCoachesWeb.Areas.Alumno.Models
{

    public class TemaHome
    {
        public Tema Tema { get; set; }
        public int Porcentaje { get; set; }
    }


        public class HomeIndexViewModel
    {
        public List<TemaHome> temasHome { get; set; }
        
        public int Puntos { get; set; }

        public int NRealizados { get; set; }
        public int NExtras { get; set; }
        public List<Examen> Examenes { get; set; }

        public List<Noticia> Noticias { get; set; }
        public List<ForoHilo> Hilos { get; set; }
        public string ColorEstilo(int porcentaje)
        {
            //success 75-100
            //warning 25-75
            //danger 0-25
            if(porcentaje<25)
            {
                return "danger";
            }
            if (porcentaje < 75)
            {
                return "warning";
            }
            return "success";
        }

        public string TextoExamen(bool aprobado)
        {
            if (aprobado)
                return "Superado";
            else
                return "No Superado";
        }

        public string ColorResultado(bool aprobado)
        {
            if (aprobado)
                return "text-success";
            else
                return "text-danger";
        }
    }
}
