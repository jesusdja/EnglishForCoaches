using EnglishForCoachesWeb.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnglishForCoachesWeb.Database.Ejercicios
{
    public class DocumentoGrupo
    {
        public int DocumentoGrupoId { get; set; }

        public int DocumentoId { get; set; }
        public virtual Documento Documento { get; set; }


        [Display(Name = "Grupo de usuarios")]
        public int GrupoUsuarioId { get; set; }

        public virtual GrupoUsuario GrupoUsuario { get; set; }

    }
}