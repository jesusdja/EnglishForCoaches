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
    public class SubTemasIndexViewModel
    {
        public List<SubTema> listadoSubTemas { get; set; }

        public Tema Tema { get; set; }
    }


    public class SubTemaViewModel
    {
        public Tema Tema { get; set; }
        public SubTema SubTema { get; set; }

        public List<SelectListItem> Clientes { get; set; }
    }


    public class SubTemasCreateViewModel : SubTemaViewModel
    {
        public void CargarClienteSeleccionado(AuthContext db)
        {
            Clientes = db.Clientes.OrderBy(st => st.Descripcion).Select(subt => new SelectListItem { Text = subt.Descripcion, Value = subt.ClienteId.ToString() }).ToList();
        }
    }

    public class SubTemasEditViewModel : SubTemaViewModel
    {
        public void CargarClienteSeleccionado(AuthContext db)
        {
            var clientesSeleccionados = db.ClienteSubTemas.Where(sau => sau.SubTemaId == SubTema.SubTemaId).Select(sau => sau.ClienteId).ToList();

            Clientes = db.Clientes.OrderBy(st => st.Descripcion).Select(subt =>
            new SelectListItem { Text = subt.Descripcion, Value = subt.ClienteId.ToString(), Selected = clientesSeleccionados.Contains(subt.ClienteId) }).ToList();
        }
    }

    public class SubTemasMoveViewModel
    {
        public SubTema SubTema { get; set; }

        public int TemaCopiarId { get; set; }

        public SelectList TemasSelectList { get; set; }
        

        public void InicializarDesplegables()
        {
            AuthContext db = new AuthContext();
            
            TemasSelectList = new SelectList(db.Temas, "TemaId", "Descripcion");
        }
    }
}