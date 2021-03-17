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
    

    public class NoticiasIndexViewModel : ViewModelPaginado
    {

        public List<Noticia> noticias { get; set; }
        
    }
    
    public class NoticiasViewViewModel
    {
        public Noticia Noticia { get; set; }

    }
    
}