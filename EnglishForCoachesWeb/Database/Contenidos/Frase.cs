using EnglishForCoachesWeb.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnglishForCoachesWeb.Database.Ejercicios
{
    public class Frase
    {        
        public int FraseId { get; set; }
        
 

        [RequiredLocalized]
        [Display(Name = "Español")]
        public string Palabra_es { get; set; }

        [RequiredLocalized]
        [Display(Name = "Ingles")]
        public string Palabra_en { get; set; }

        [Display(Name = "Fichero audio español")]
        public string FicheroAudio_es { get; set; }

        [Display(Name = "Fichero audio ingles")]
        public string FicheroAudio_en { get; set; }


        [Display(Name = "Gramática")]
        public int GramaticaId { get; set; }
        public virtual Gramatica Gramatica { get; set; }

        [Display(Name = "Comentario")]
        public string Comentario { get; set; }


        [Display(Name = "Vocabulario")]
        public virtual ICollection<Vocabulario> Vocabularios { get; set; }

    }
}