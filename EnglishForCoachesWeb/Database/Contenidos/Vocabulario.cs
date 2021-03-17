using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnglishForCoachesWeb.Database.Ejercicios
{
    public class Vocabulario
    {
        public int VocabularioId { get; set; }

        [Required]
        [Display(Name = "Inglés")]
        public string Palabra_en { get; set; }

        [Required]
        [Display(Name = "Español")]
        public string Palabra_es { get; set; }
        
        [Display(Name = "Definición")]
        public string Definicion { get; set; }


        public virtual List<DefinicionVocabulario> DefinicionVocabularios { get; set; }


        [Display(Name = "Fichero audio")]
        public string FicheroAudio { get; set; }


        public int CategoriaVocabularioId { get; set; }
        public virtual CategoriaVocabulario CategoriaVocabulario { get; set; }

        [Display(Name = "Explicación")]
        public string Explicacion { get; set; }
        public virtual ICollection<Gramatica> Gramaticas { get; set; }


        public virtual ICollection<Frase> Frases { get; set; }
    }
}