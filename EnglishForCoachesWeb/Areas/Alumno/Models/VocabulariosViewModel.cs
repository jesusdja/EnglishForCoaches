using EnglishForCoachesWeb.Database.Ejercicios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnglishForCoachesWeb.Areas.Alumno.Models
{

    public class VocabulariosIndexViewModel
    {
        public List<string> letras = new List<string>()
        {
            "A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z"
        };
        public List<Vocabulario> vocabulario{ get; set; }
        public string Idioma { get; set; }
        public string Letra { get; set; }

        [StringLength(250, MinimumLength = 3, ErrorMessage = "Mínimo 3 caracteres")]
        public string TextoBusqueda { get; set; }

        public List<int> glosario { get; set; }
    }
    public class VocabulariosGramaticaViewModel
    {
        public List<Vocabulario> vocabulario { get; set; }
        public SubTema Subtema { get; set; }
        public Gramatica Gramatica { get; set; }

        
        public string Idioma { get; set; }

        public List<int> glosario { get; set; }
    }

}