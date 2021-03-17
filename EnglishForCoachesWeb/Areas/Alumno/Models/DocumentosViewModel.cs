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
    

    public class DocumentosIndexViewModel
    {

        public List<Documento> Documentos { get; set; }

        public List<Tema> Temas { get; set; }


        public List<SubTema> Subtemas { get; set; }
    }

    public class DocumentosViewViewModel
    {
        public Documento Documento { get; set; }

    }
    
}