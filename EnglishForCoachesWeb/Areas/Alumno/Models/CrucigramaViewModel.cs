using EnglishForCoachesWeb.Database;
using EnglishForCoachesWeb.Database.Ejercicios;
using EnglishForCoachesWeb.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace EnglishForCoachesWeb.Areas.Alumno.Models
{
    public class CrucigramaIndexViewModel
    {
        public Bloque bloque { get; set; }

        public string[][] Letras { get; set; }

        
        public string[][] LetrasRespuesta { get; set; }

        public Crucigrama Crucigrama { get; set; }
    }

    public class CrucigramaResultado
    {
        public bool[][] Correctas { get; set; }
        
    }
}