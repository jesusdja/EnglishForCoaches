using EnglishForCoachesWeb.Database;
using EnglishForCoachesWeb.Database.Ejercicios;
using EnglishForCoachesWeb.Database.Varios;
using EnglishForCoachesWeb.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace EnglishForCoachesWeb.Areas.Admin.Models
{
    public class ErroresExamenIndexViewModel : ViewModelPaginado
    {
        public List<Examen> listadoExamenes { get; set; }
   
        public string TextoBusqueda { get; set; }

    }

        public class ErroresExamenViewModel
    {

        public Examen Examen { get; set; }
    }

    public class ErroresExamenCreateViewModel: ErroresExamenViewModel
    {
    }

        public class ErroresExamenEditViewModel: ErroresExamenViewModel
    {
    }
}