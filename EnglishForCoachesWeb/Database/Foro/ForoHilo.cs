using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnglishForCoachesWeb.Database.Foro
{
    public class ForoHilo
    {
        public int ForoHiloId { get; set; }
        [Required]
        public string Titulo { get; set; }

        [Required]
        [AllowHtml]
        public string Texto { get; set; }


        public int ClienteId { get; set; }

        public DateTime FechaCreacion { get; set; }
        public string CreadoPor { get; set; }
        public string AlumnoId { get; set; }

        public string AlumnoRespuestaId { get; set; }
        public DateTime FechaRespuesta { get; set; }
        public string RespondidoPor { get; set; }
        public bool RespondidoPorAdmin { get; set; }

        public int ForoCategoriaId { get; set; }
        public virtual ForoCategoria ForoCategoria { get; set; }

        public bool Admin { get; set; }

        public virtual ICollection<ForoMensaje> Mensajes { get; set; }
    }
}