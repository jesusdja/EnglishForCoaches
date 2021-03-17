using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnglishForCoachesWeb.Database.Foro
{
    public class ForoMensaje
    {
        public int ForoMensajeId { get; set; }

        [Required]
        [AllowHtml]
        public string Texto { get; set; }

        public DateTime FechaCreacion { get; set; }
        public string CreadoPor { get; set; }
        public string AlumnoId { get; set; }

        public bool Admin { get; set; }
        public int ForoHiloId { get; set; }

    }
}