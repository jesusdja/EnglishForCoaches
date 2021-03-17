using EnglishForCoachesWeb.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnglishForCoachesWeb.Database.Varios
{
    public class Noticia
    {
        public int NoticiaId { get; set;}
        [Required]
        public string Titulo { get; set; }

        public int ClienteId { get; set; }

        [Required]
        [AllowHtml]
        public string Texto { get; set; }

        public string TextoResumen { get; set; }
        public string Foto { get; set; }

        public string FicheroAdjunto { get; set; }


        public string UsuarioId { get; set; }

        public DateTime Fecha { get; set; }

        public virtual ICollection<NoticiaGrupo> NoticiaGrupos { get; set; }
        

    }
}