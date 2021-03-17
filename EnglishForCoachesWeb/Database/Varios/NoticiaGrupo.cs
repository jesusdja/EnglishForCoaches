using EnglishForCoachesWeb.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnglishForCoachesWeb.Database.Varios
{
    public class NoticiaGrupo
    {
        public int NoticiaGrupoId { get; set; }

        public int NoticiaId { get; set; }
        public virtual Noticia Noticia { get; set; }


        [Display(Name = "Grupo de usuarios")]
        public int GrupoUsuarioId { get; set; }

        public virtual GrupoUsuario GrupoUsuario { get; set; }

    }
}