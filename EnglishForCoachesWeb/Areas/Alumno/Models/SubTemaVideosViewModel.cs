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


    public class SubTemaVideosIndexViewModel
    {
        public SubTema SubTema { get; set; }

        public List<SubTemaVideo> SubTemaVideos { get; set; }

    }
    
    
}