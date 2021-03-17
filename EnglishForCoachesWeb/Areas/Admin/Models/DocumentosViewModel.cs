using EnglishForCoachesWeb.Database;
using EnglishForCoachesWeb.Database.Ejercicios;
using EnglishForCoachesWeb.Database.Varios;
using EnglishForCoachesWeb.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace EnglishForCoachesWeb.Areas.Admin.Models
{
    public class DocumentoIndexViewModel : ViewModelPaginado
    {
        public List<Documento> listadoDocumentos { get; set; }
   
        public string TextoBusqueda { get; set; }

    }


        public class DocumentoViewModel
    {


        public Documento Documento { get; set; }

        public List<GrupoUsuarioSeleccionado> GruposUsuarios { get; set; }

        public SelectList TemasSelectList { get; set; }
        public SelectList SubTemasSelectList { get; set; }

        public List<Tema> TemaList { get; set; }
        public List<SubTema> SubTemaList { get; set; }

        public void InicializarDesplegables()
        {
            AuthContext db = new AuthContext();

            var grupos = db.GrupoUsuarios.OrderBy(t => t.Descripcion).ToList();
            GruposUsuarios = new List<GrupoUsuarioSeleccionado>();
            foreach (var grupo in grupos)
            {
                GruposUsuarios.Add(new GrupoUsuarioSeleccionado()
                {
                    Descripcion = grupo.Descripcion,
                    Id = grupo.GrupoUsuarioId,
                    Seleccionado = false
                });
            }
            TemaList = db.Temas.ToList();
            SubTemaList = db.SubTemas.OrderBy(t => t.Orden).ToList();
            TemasSelectList = new SelectList(db.Temas, "TemaId", "Descripcion");
            SubTemasSelectList = new SelectList(db.SubTemas.OrderBy(t => t.Descripcion), "SubTemaId", "Descripcion");
        }
    }

    public class DocumentoCreateViewModel: DocumentoViewModel
    {
        [RequiredLocalized]
        [Display(Name = "Fichero adjunto")]
        [MaxFileSize(5 * 1024 * 1024, ErrorMessage = "El tamaño máximo permitido es de {0} bytes")]
        public virtual HttpPostedFileBase File { get; set; }
    }

        public class DocumentoEditViewModel: DocumentoViewModel
    {

        [Display(Name = "Fichero adjunto")]
        [MaxFileSize(5 * 1024 * 1024, ErrorMessage = "El tamaño máximo permitido es de {0} bytes")]
        public virtual HttpPostedFileBase File { get; set; }
    }
}