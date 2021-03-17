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
    public class JuegoIndexViewModel: ViewModelPaginado
    {
        public SubTema Subtema { get; set; }
        public List<Juego> listadoJuegos { get; set; }


        public string TextoBusqueda { get; set; }

    }

    public class JuegoViewModel
    {
        public SubTema Subtema { get; set; }

        [Display(Name = "Fichero")]
        [MaxFileSize(5 * 1024 * 1024, ErrorMessage = "El tamaño máximo permitido es de {0} bytes")]
        public virtual HttpPostedFileBase File { get; set; }

        public Juego Juego { get; set; }

        public SelectList categoriaJuegosSelectList { get; set; }
        public List<SelectListItem> Clientes { get; set; }
        public void InicializarDesplegables()
        {
            AuthContext db = new AuthContext();

            categoriaJuegosSelectList = new SelectList(db.CategoriaJuegos.OrderBy(t => t.Descripcion), "CategoriaJuegoId", "Descripcion");
        }
    }

    public class JuegoCreateViewModel: JuegoViewModel
    {
        public void CargarClienteSeleccionado(AuthContext db)
        {
            Clientes = db.Clientes.OrderBy(st => st.Descripcion).Select(subt => new SelectListItem { Text = subt.Descripcion, Value = subt.ClienteId.ToString() }).ToList();
        }
    }

        public class JuegoEditViewModel: JuegoViewModel
    {
        public void CargarClienteSeleccionado(AuthContext db)
        {
            var clientesSeleccionados = db.ClienteJuegos.Where(sau => sau.JuegoId == Juego.JuegoId).Select(sau => sau.ClienteId).ToList();

            Clientes = db.Clientes.OrderBy(st => st.Descripcion).Select(subt =>
            new SelectListItem { Text = subt.Descripcion, Value = subt.ClienteId.ToString(), Selected = clientesSeleccionados.Contains(subt.ClienteId) }).ToList();
        }
    }
}