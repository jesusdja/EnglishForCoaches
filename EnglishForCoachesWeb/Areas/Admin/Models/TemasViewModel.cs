using EnglishForCoachesWeb.Database;
using EnglishForCoachesWeb.Database.Clientes;
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

    public class TemaIndexViewModel : ViewModelPaginado
    {
        public List<Tema> listadoTemas { get; set; }

        public string TextoBusqueda { get; set; }

    }


    public  class TemaViewModel
    {
        public Tema Tema { get; set; }

        public List<SelectListItem> Clientes { get; set; }
    }

    public class TemaCreateViewModel : TemaViewModel
    {
        public  void CargarClienteSeleccionado(AuthContext db)
        {
            Clientes = db.Clientes.OrderBy(st => st.Descripcion).Select(subt => new SelectListItem { Text = subt.Descripcion, Value = subt.ClienteId.ToString() }).ToList();
        }
    }

    public class TemaEditViewModel : TemaViewModel
    {
        public  void CargarClienteSeleccionado(AuthContext db)
        {
            var clientesSeleccionados = db.ClienteTemas.Where(sau => sau.TemaId == Tema.TemaId).Select(sau => sau.ClienteId).ToList();

            Clientes = db.Clientes.OrderBy(st => st.Descripcion).Select(subt => 
            new SelectListItem { Text = subt.Descripcion, Value = subt.ClienteId.ToString(), Selected = clientesSeleccionados.Contains(subt.ClienteId) }).ToList();
        }
    }

    public class TemasVideoViewModel
    {
        public Tema Tema { get; set; }
        
    }
    
}