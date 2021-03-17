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
    public class BloquesIndexViewModel
    {
        public List<Gramatica> listadoGramaticas { get; set; }
        public List<Bloque> listadoBloques { get; set; }
                public List<Juego> listadoJuegos { get; set; }
        public List<JuegoOnline> listadoJuegoOnlines { get; set; }
        public List<SubTemaVideo> listadoSubTemaVideos { get; set; }
        public List<Extra> listadoExtras { get; set; }
        
        public SubTema Subtema { get; set; }
        
        public int? TipoEjercicioBusquedaId { get; set; }
        public int? AreaBusquedaId { get; set; }   
        public string TextoBusqueda { get; set; }

        public SelectList tipoEjerciciosSelectList { get; set; }
        public SelectList areasSelectList { get; set; }
        public int pestanyaSeleccionada { get; set; }
        public void InicializarDesplegables()
        {
            AuthContext db = new AuthContext();

            areasSelectList = new SelectList(db.Areas.OrderBy(a => a.Descripcion), "AreaId", "Descripcion");
            tipoEjerciciosSelectList = new SelectList(db.TipoEjercicios.OrderBy(t => t.Descripcion), "TipoEjercicioId", "Descripcion");
        }
    }

    public class BloquesViewModel
    {
        public Bloque Bloque { get; set; }
        public List<SelectListItem> Clientes { get; set; }
    }

    public class BloquesCreateViewModel: BloquesViewModel
    {
        public SubTema Subtema { get; set; }


        public SelectList tipoEjerciciosSelectList { get; set; }
        public SelectList areasSelectList { get; set; }
        public void InicializarDesplegables()
        {
            AuthContext db = new AuthContext();
            
            tipoEjerciciosSelectList = new SelectList(db.TipoEjercicios.OrderBy(t => t.Descripcion), "TipoEjercicioId", "Descripcion");
            areasSelectList = new SelectList(db.Areas.OrderBy(t => t.Descripcion), "AreaId", "Descripcion");
        }
        public void CargarClienteSeleccionado(AuthContext db)
        {
            Clientes = db.Clientes.OrderBy(st => st.Descripcion).Select(subt => new SelectListItem { Text = subt.Descripcion, Value = subt.ClienteId.ToString() }).ToList();
        }
    }

    public class BloquesEditViewModel: BloquesViewModel
    {

        public void CargarClienteSeleccionado(AuthContext db)
        {
            var clientesSeleccionados = db.ClienteBloques.Where(sau => sau.BloqueId == Bloque.BloqueId).Select(sau => sau.ClienteId).ToList();

            Clientes = db.Clientes.OrderBy(st => st.Descripcion).Select(subt =>
            new SelectListItem { Text = subt.Descripcion, Value = subt.ClienteId.ToString(), Selected = clientesSeleccionados.Contains(subt.ClienteId) }).ToList();
        }
    }
    public class BloquesCopyViewModel
    {
        public Bloque Bloque { get; set; }

        public int SubTemaCopiarId { get; set; }

        public SelectList SubTemasSelectList { get; set; }

        public List<Tema> TemaList { get; set; }
        public List<SubTema> SubTemaList { get; set; }

        public int  AreaCopiarId { get; set; }

        public SelectList AreasSelectList { get; set; }
        public void InicializarDesplegables()
        {
            AuthContext db = new AuthContext();

            TemaList=db.Temas.ToList();
            SubTemaList = db.SubTemas.OrderBy(t => t.Orden).ToList();
            SubTemasSelectList = new SelectList(db.SubTemas.OrderBy(t => t.Orden), "SubTemaId", "Descripcion");
            AreasSelectList = new SelectList(db.Areas.OrderBy(t => t.Descripcion), "AreaId", "Descripcion");
        }
    }
}