using EnglishForCoachesWeb.Database;
using EnglishForCoachesWeb.Database.Ejercicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace EnglishForCoachesWeb.Areas.Admin.Models
{


    public class GramaticaViewModel
    {
        public Gramatica Gramatica { get; set; }
        public Tema Tema { get; set; }

        public SubTema SubTema { get; set; }
        public List<SelectListItem> Clientes { get; set; }
    }

    public class GramaticasCreateViewModel: GramaticaViewModel
    {
        public void CargarClienteSeleccionado(AuthContext db)
        {
            Clientes = db.Clientes.OrderBy(st => st.Descripcion).Select(subt => new SelectListItem { Text = subt.Descripcion, Value = subt.ClienteId.ToString() }).ToList();
        }
    }

    public class GramaticasEditViewModel : GramaticaViewModel
    {
        public void CargarClienteSeleccionado(AuthContext db)
        {
            var clientesSeleccionados = db.ClienteGramaticas.Where(sau => sau.GramaticaId == Gramatica.GramaticaId).Select(sau => sau.ClienteId).ToList();

            Clientes = db.Clientes.OrderBy(st => st.Descripcion).Select(subt =>
            new SelectListItem { Text = subt.Descripcion, Value = subt.ClienteId.ToString(), Selected = clientesSeleccionados.Contains(subt.ClienteId) }).ToList();
        }
    }

    public class GramaticasMoveViewModel
    {
        public SubTema SubTema { get; set; }
        public Gramatica Gramatica { get; set; }

        public int SubTemaCopiarId { get; set; }

        public SelectList SubTemasSelectList { get; set; }

        public List<Tema> TemaList { get; set; }
        public List<SubTema> SubTemaList { get; set; }
        
        public void InicializarDesplegables()
        {
            AuthContext db = new AuthContext();

            TemaList = db.Temas.ToList();
            SubTemaList = db.SubTemas.OrderBy(t => t.Orden).ToList();
            SubTemasSelectList = new SelectList(db.SubTemas.OrderBy(t => t.Descripcion), "SubTemaId", "Descripcion");
        }
    }
}