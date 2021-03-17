using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnglishForCoachesWeb.Database.Ejercicios
{
    public class Tema
    {
        public int TemaId { get; set; }

        [Display(Name = "Tema")]
        public string Descripcion { get; set; }
        [AllowHtml]
        public string UrlVideo { get; set; }

        public string Introduccion { get; set; }
        public string IconoAdmin { get; set; }

        public string IconoUsuario { get; set; }
    }
    public enum TemasId
    {
        Gramatica = 1,
        UnidadesDidacticas = 2,
        EnglishForFootball = 3
    }

}