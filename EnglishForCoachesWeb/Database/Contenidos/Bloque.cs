using EnglishForCoachesWeb.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnglishForCoachesWeb.Database.Ejercicios
{
    public class Bloque
    {
        public int BloqueId { get; set; }

        [RequiredLocalized]
        [Display(Name = "Descripcion")]
        public string Descripcion { get; set; }

        [Display(Name = "Explicación")]
        public string Explicacion { get; set; }


        [Display(Name = "SubTema")]
        public int SubTemaId { get; set; }
        public virtual SubTema SubTema { get; set; }

        [Display(Name = "Area")]
        public int AreaId { get; set; }
        public virtual Area Area { get; set; }
        
        [Display(Name = "TipoEjercicio")]
        public int TipoEjercicioId { get; set; }
        public virtual TipoEjercicio TipoEjercicio { get; set; }

        
        public virtual ICollection<Test> Tests { get; set; }
        public virtual ICollection<Skillwise> Skillwises { get; set; }
        public virtual ICollection<WordByWord> WordByWords { get; set; }
        public virtual ICollection<MatchTheWord> MatchTheWords { get; set; }
        public virtual ICollection<FillTheGap> FillTheGaps { get; set; }
        public virtual ICollection<FillTheBox> FillTheBoxs { get; set; }
        public virtual ICollection<AudioSentenceExercise> AudioSentenceExercises { get; set; }
        public virtual ICollection<Crucigrama> Crucigramas { get; set; }
        public virtual ICollection<MatchThePicture> MatchThePictures { get; set; }
        public virtual ICollection<TrueFalse> TrueFalses { get; set; }
    }
}