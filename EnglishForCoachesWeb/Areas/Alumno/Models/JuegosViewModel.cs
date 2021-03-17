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


    public class JuegosIndexViewModel
    {
        public SubTema SubTema { get; set; }

        public List<CategoriaJuego> CategoriaJuegos { get; set; }

    }

    public class JuegosCategoriaViewModel
    {
        public SubTema SubTema { get; set; }
        public CategoriaJuego CategoriaJuego { get; set; }

        public List<Juego> Juegos { get; set; }
        
    }

    
    public class JuegosViewViewModel
    {
        public SubTema SubTema { get; set; }
        public Juego Juego { get; set; }

    }
    
}