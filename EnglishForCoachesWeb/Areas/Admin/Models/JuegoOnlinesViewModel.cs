using EnglishForCoachesWeb.Database;
using EnglishForCoachesWeb.Database.Ejercicios;
using EnglishForCoachesWeb.Database.JuegoOnline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace EnglishForCoachesWeb.Areas.Admin.Models
{ 

    public class JuegoOnlinesCreateViewModel
    {
        public SubTema Subtema { get; set; }

        public JuegoOnline JuegoOnline { get; set; }
        public List<SelectListItem> Clientes { get; set; }

        public SelectList tipoJuegoOnlinesSelectList { get; set; }
        public void InicializarDesplegables()
        {
            AuthContext db = new AuthContext();
            
            tipoJuegoOnlinesSelectList = new SelectList(db.TipoJuegoOnlines.OrderBy(t => t.Descripcion), "TipoJuegoOnlineId", "Descripcion");
        }
        public void CargarClienteSeleccionado(AuthContext db)
        {
            Clientes = db.Clientes.OrderBy(st => st.Descripcion).Select(subt => new SelectListItem { Text = subt.Descripcion, Value = subt.ClienteId.ToString() }).ToList();
        }
    }

    public class JuegoOnlinesEditViewModel
    {
        public JuegoOnline JuegoOnline { get; set; }
        public List<SelectListItem> Clientes { get; set; }

        public void CargarClienteSeleccionado(AuthContext db)
        {
            var clientesSeleccionados = db.ClienteJuegoOnlines.Where(sau => sau.JuegoOnlineId == JuegoOnline.JuegoOnlineId).Select(sau => sau.ClienteId).ToList();

            Clientes = db.Clientes.OrderBy(st => st.Descripcion).Select(subt =>
            new SelectListItem { Text = subt.Descripcion, Value = subt.ClienteId.ToString(), Selected = clientesSeleccionados.Contains(subt.ClienteId) }).ToList();
        }
    }
    public class JuegoOnlinesCopyViewModel
    {
        public JuegoOnline JuegoOnline { get; set; }

        public int SubTemaCopiarId { get; set; }

        public SelectList SubTemasSelectList { get; set; }

        public List<Tema> TemaList { get; set; }
        public List<SubTema> SubTemaList { get; set; }

        public void InicializarDesplegables()
        {
            AuthContext db = new AuthContext();

            TemaList=db.Temas.ToList();
            SubTemaList = db.SubTemas.OrderBy(t => t.Orden).ToList();
            SubTemasSelectList = new SelectList(db.SubTemas.OrderBy(t => t.Orden), "SubTemaId", "Descripcion");
        }
    }
}