using System.ComponentModel.DataAnnotations;

namespace EnglishForCoachesWeb.Database.Ejercicios
{
    public class DefinicionVocabulario
    {
        public int DefinicionVocabularioId { get; set; }
        public int VocabularioId { get; set; }

        [Required]
        [Display(Name = "Español")]
        public string Palabra_es { get; set; }

        [Display(Name = "Definición")]
        public string Definicion { get; set; }
    }
}