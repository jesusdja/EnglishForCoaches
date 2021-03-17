using EnglishForCoachesWeb.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnglishForCoachesWeb.Database.Ejercicios
{
    public class Documento
    {
        public int DocumentoId { get; set;}
        [Required]
        public string Titulo { get; set; }

        [Required]
        public string Descripcion { get; set; }
 
        public string FicheroAdjunto { get; set; }

        [Display(Name = "Tema")]
        public int? TemaId { get; set; }
        public virtual Tema Tema { get; set; }

        [Display(Name = "Subtema")]
        public int? SubTemaId { get; set; }
        public virtual SubTema SubTema { get; set; }

        public DateTime Fecha { get; set; }

        public virtual ICollection<DocumentoGrupo> DocumentoGrupos { get; set; }
        

    }
}