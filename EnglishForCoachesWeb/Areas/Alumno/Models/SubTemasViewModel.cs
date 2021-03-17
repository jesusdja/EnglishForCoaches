using EnglishForCoachesWeb.Database;
using EnglishForCoachesWeb.Database.Ejercicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EnglishForCoachesWeb.Areas.Alumno.Models
{
    public class SubTemasIndexViewModel
    {
        public List<int> examenesDesbloqueados { get; set; }
        public List<SubTema> listadoSubTemasDesbloqueados { get; set; }

        public List<SubTema> listadoSubTemasBloqueados { get; set; }

        public List<int> listadoSubTemasAcceso { get; set; }

        public Tema Tema { get; set; }

        public int NLecciones { get; set; }
        public int NUnidades { get; set; }
        public int NPracticas { get; set; }
        public int NVocabulario { get; set; }
        public int NFrases { get; set; }

        public bool TemaSuperado { get; set; }
    }


    public class SubTemasViewViewModel
    {
        public List<AreaContenidos> listadoAreas { get; set; }
        public Tema Tema { get; set; }
        public SubTema SubTema { get; set; }

        public List<Gramatica> Gramaticas { get; set; }

        public Gramatica GramaticaMostrar { get; set; }
        
        public bool MostrarExamen { get; set; }
        public List<int> examenesDesbloqueados { get; set; }

        
    }

    public class AreaContenidos
    {
        public Area Area { get; set; }
        public int NumContenidos { get; set; }
    }


}