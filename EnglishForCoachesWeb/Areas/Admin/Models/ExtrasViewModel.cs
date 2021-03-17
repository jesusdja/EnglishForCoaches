using EnglishForCoachesWeb.Database;
using EnglishForCoachesWeb.Database.Ejercicios;
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
    public class ExtraIndexViewModel : ViewModelPaginado
    {
        public List<Extra> listadoExtras { get; set; }
   
        public string TextoBusqueda { get; set; }

        public int CategoriaBusquedaId { get; set; }
        public SelectList categoriasSelectList { get; set; }
        public void InicializarDesplegables()
        {
            AuthContext db = new AuthContext();

            categoriasSelectList = new SelectList(db.CategoriaExtras.OrderBy(a => a.Descripcion), "CategoriaextraId", "Descripcion");
        }
    }

    public class ExtraViewModel
    {
        public SubTema Subtema { get; set; }

        [Display(Name = "Fichero de audio")]
        [MaxFileSize(5 * 1024 * 1024, ErrorMessage = "El tamaño máximo permitido es de {0} bytes")]
        public virtual HttpPostedFileBase AudioFile { get; set; }

        [Display(Name = "Imagen")]
        [MaxFileSize(3 * 1024 * 1024, ErrorMessage = "El tamaño máximo permitido es de {0} bytes")]
        [MinImageSizeAttribute(800, 0, ErrorMessage = "El tamaño mínimo de la imagen es de {0}x{1} pixels")]
        public virtual HttpPostedFileBase ImageFile { get; set; }

        public Extra Extra { get; set; }
        public List<SelectListItem> Clientes { get; set; }

        public SelectList categoriaExtrasSelectList { get; set; }
        public void InicializarDesplegables()
        {
            AuthContext db = new AuthContext();

            categoriaExtrasSelectList = new SelectList(db.CategoriaExtras.OrderBy(t => t.Descripcion), "CategoriaExtraId", "Descripcion");
        }
    }

    public class ExtraCreateViewModel: ExtraViewModel
    {
        public void CargarClienteSeleccionado(AuthContext db)
        {
            Clientes = db.Clientes.OrderBy(st => st.Descripcion).Select(subt => new SelectListItem { Text = subt.Descripcion, Value = subt.ClienteId.ToString() }).ToList();
        }
    }

        public class ExtraEditViewModel: ExtraViewModel
    {
        public void CargarClienteSeleccionado(AuthContext db)
        {
            var clientesSeleccionados = db.ClienteExtras.Where(sau => sau.ExtraId == Extra.ExtraId).Select(sau => sau.ClienteId).ToList();

            Clientes = db.Clientes.OrderBy(st => st.Descripcion).Select(subt =>
            new SelectListItem { Text = subt.Descripcion, Value = subt.ClienteId.ToString(), Selected = clientesSeleccionados.Contains(subt.ClienteId) }).ToList();
        }
    }

    public class ExtraCopyViewModel
    {
        public Extra Extra { get; set; }

        public int SubTemaCopiarId { get; set; }

        public SelectList SubTemasSelectList { get; set; }

        public List<Tema> TemaList { get; set; }
        public List<SubTema> SubTemaList { get; set; }

        public void InicializarDesplegables()
        {
            AuthContext db = new AuthContext();

            TemaList = db.Temas.ToList();
            SubTemaList = db.SubTemas.OrderBy(t => t.Orden).ToList();
            SubTemasSelectList = new SelectList(db.SubTemas.OrderBy(t => t.Orden), "SubTemaId", "Descripcion");
        }
    }
}