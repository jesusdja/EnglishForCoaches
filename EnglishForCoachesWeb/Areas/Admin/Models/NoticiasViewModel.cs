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
    public class NoticiaIndexViewModel : ViewModelPaginado
    {
        public List<Noticia> listadoNoticias { get; set; }
   
        public string TextoBusqueda { get; set; }

    }

        public class NoticiaViewModel
    {

        [Display(Name = "Fichero adjunto")]
        [MaxFileSize(5 * 1024 * 1024, ErrorMessage = "El tamaño máximo permitido es de {0} bytes")]
        public virtual HttpPostedFileBase File { get; set; }

        [Display(Name = "Imagen")]
        [MaxFileSize(3 * 1024 * 1024, ErrorMessage = "El tamaño máximo permitido es de {0} bytes")]
        [MinImageSizeAttribute(800, 640, ErrorMessage = "El tamaño mínimo de la imagen es de {0}x{1} pixels")]
        public virtual HttpPostedFileBase ImageFile { get; set; }

        public Noticia Noticia { get; set; }

        public List<GrupoUsuarioSeleccionado> GruposUsuarios { get; set; }
        public SelectList ClienteSelectList { get; set; }

        public void InicializarDesplegables()
        {
            AuthContext db = new AuthContext();

            ClienteSelectList = new SelectList(db.Clientes, "ClienteId", "Descripcion");
            var grupos = db.GrupoUsuarios.OrderBy(t => t.Descripcion).ToList();
            GruposUsuarios = new List<GrupoUsuarioSeleccionado>();
            foreach (var grupo in grupos)
            {
                GruposUsuarios.Add(new GrupoUsuarioSeleccionado() {
                Descripcion=grupo.Descripcion,
                Id= grupo.GrupoUsuarioId,
                Seleccionado=false
                });
            }
        }
    }

    public class NoticiaCreateViewModel: NoticiaViewModel
    {
    }

        public class NoticiaEditViewModel: NoticiaViewModel
    {
    }
}