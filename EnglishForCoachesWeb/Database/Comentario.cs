using EnglishForCoachesWeb.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnglishForCoachesWeb.Database
{
    public class Comentario
    {
        public int ComentarioId { get; set; }

        public int ArticuloId { get; set; }
        public virtual Articulo Articulo { get; set; }

        [Display(Name = "Aceptado")]
        [RequiredLocalized]
        public bool Aceptado { get; set; }

        [Display(Name = "Comentario")]
        [RequiredLocalized]
        public string Texto { get; set; }

        [Display(Name = "Nombre")]
        [RequiredLocalized]
        public string Nombre { get; set; }

        [Display(Name = "E-mail")]
        [RequiredLocalized]
        public string Email { get; set; }

        [Display(Name = "Fecha comentario")]
        [RequiredLocalized]
        public DateTime FechaHoraComentario { get; set; }

    }
}