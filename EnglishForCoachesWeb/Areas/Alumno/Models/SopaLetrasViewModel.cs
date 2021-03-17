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
using EnglishForCoachesWeb.Database.JuegoOnline;

namespace EnglishForCoachesWeb.Areas.Alumno.Models
{
    public class SopaLetrasIndexViewModel
    {
        public JuegoOnline juegoOnline { get; set; }

        public string[][] Letras { get; set; }


        public List<string> Vocabularios { get; set; }

        public List<DefinicionSopaLetras> Definiciones { get; set; }

        public string[][] LetrasRespuesta { get; set; }

        public SopaLetras SopaLetras { get; set; }
    }
    public class SopaLetrasCompletadoViewModel
    {
        public JuegoOnline juegoOnline { get; set; }


        public List<Vocabulario> vocabulario { get; set; }
        public string Idioma { get; set; }

        public List<int> glosario { get; set; }

    }
    
    public class SopaLetrasResultado
    {
        public bool Correcto { get; set; }
        public bool Finalizado { get; set; }
        public string Comentario { get; set; }
        public string textoFinalizado { get; set; }
        public int VocabularioId { get; set; }
        public string FicheroAudio { get; set; }
        
    }
    public class DefinicionSopaLetras
    {
        public int VocabularioId { get; set; }
        public string Definicion { get; set; }

    }
}