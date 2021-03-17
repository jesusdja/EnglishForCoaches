using EnglishForCoachesWeb.Database.Ejercicios;
using EnglishForCoachesWeb.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace EnglishForCoachesWeb.Areas.Admin.Models
{
    public class GramaticaVocabularioIndexViewModel
    {
        public List<Vocabulario> listadoGramaticaVocabularios { get; set; }
    
        public Gramatica Gramatica { get; set; }

    }

    public class GramaticaVocabularioCreateViewModel : ViewModelPaginado
    {
        public List<int> listadoVocabularioIncluidos { get; set; }
        public List<Vocabulario> listadoVocabularios { get; set; }
        public Gramatica Gramatica { get; set; }
        public int Pagina { get; set; }

        public int PaginaMin { get; set; }
        public int PaginaMax { get; set; }
        public int NPaginas { get; set; }

        public string TextoBusqueda { get; set; }

    }
}