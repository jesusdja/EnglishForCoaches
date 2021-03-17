using EnglishForCoachesWeb.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnglishForCoachesWeb.Database.Varios
{
    public class Video
    {
        public int VideoId { get; set;}
        [Required]
        public string Titulo { get; set; }


        public string UrlVideo { get; set; }

        public string ThumbNail { get; set; }

        public DateTime Fecha { get; set; }
               

    }
}