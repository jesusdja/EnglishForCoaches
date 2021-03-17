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
    public class MatchThePictureIndexViewModel
    {
        public Bloque bloque { get; set; }

        public List<MatchThePicture> MatchThePictures { get; set; }
        public List<string> Imagenes { get; set; }
        public List<string> Palabras { get; set; }
    }

    public class MatchThePictureResultado
    {
        public bool[] Correctas { get; set; }
        
    }
}