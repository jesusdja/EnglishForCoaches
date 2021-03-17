using EnglishForCoachesWeb.Database;
using EnglishForCoachesWeb.Database.Ejercicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using System.Data.Entity;
using EnglishForCoachesWeb.Database.Foro;
using System.ComponentModel.DataAnnotations;

namespace EnglishForCoachesWeb.Areas.Alumno.Models
{
    public class ForoIndexViewModel
    {
        [StringLength(250, MinimumLength = 3, ErrorMessage = "Mínimo 3 caracteres")]
        public string TextoBusqueda { get; set; }

        public List<ForoCategoria> categoriasLateral { get; set; }
        public List<ForoCategoria> categorias { get; set; }

        public List<ForoHilo> hilos { get; set; }
    }
    public class ForoCreateViewModel
    {       
        public ForoCategoria ForoCategoria { get; set; }
        public ForoHilo hilo { get; set; }
    }
    public class ForoHiloViewModel
    {
        public ForoMensaje ForoMensaje { get; set; }
        public ForoHilo hilo { get; set; }
    }
        
}
