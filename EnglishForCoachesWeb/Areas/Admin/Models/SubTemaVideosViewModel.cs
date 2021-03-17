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
    public class SubTemaVideoIndexViewModel: ViewModelPaginado
    {
        public SubTema Subtema { get; set; }
        public List<SubTemaVideo> listadoSubTemaVideos { get; set; }

        public string TextoBusqueda { get; set; }
    }

    public class SubTemaVideoViewModel
    {
        public SubTema Subtema { get; set; }
        public SubTemaVideo SubTemaVideo { get; set; }
        public List<SelectListItem> Clientes { get; set; }
    }

    public class SubTemaVideoCreateViewModel: SubTemaVideoViewModel
    {
        public void CargarClienteSeleccionado(AuthContext db)
        {
            Clientes = db.Clientes.OrderBy(st => st.Descripcion).Select(subt => new SelectListItem { Text = subt.Descripcion, Value = subt.ClienteId.ToString() }).ToList();
        }
    }

        public class SubTemaVideoEditViewModel: SubTemaVideoViewModel
    {
        public void CargarClienteSeleccionado(AuthContext db)
        {
            var clientesSeleccionados = db.ClienteSubTemaVideos.Where(sau => sau.SubTemaVideoId == SubTemaVideo.SubTemaVideoId).Select(sau => sau.ClienteId).ToList();

            Clientes = db.Clientes.OrderBy(st => st.Descripcion).Select(subt =>
            new SelectListItem { Text = subt.Descripcion, Value = subt.ClienteId.ToString(), Selected = clientesSeleccionados.Contains(subt.ClienteId) }).ToList();
        }
    }
}