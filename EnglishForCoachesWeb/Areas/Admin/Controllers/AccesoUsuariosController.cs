using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EnglishForCoachesWeb.Database;
using EnglishForCoachesWeb.Database.Ejercicios;
using EnglishForCoachesWeb.Areas.Admin.Models;
using EnglishForCoachesWeb.Controllers;

namespace EnglishForCoachesWeb.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AccesoUsuariosController : MVCBaseController
    {
        private AuthContext db = new AuthContext();
        // GET: Admin/AccesoUsuarios
        public ActionResult Index()
        {           
            var AccesoUsuarios = db.AccesoUsuarios.OrderByDescending(au=>au.FechaAcceso);      


            AccesoUsuariosIndexViewModel viewModel = new AccesoUsuariosIndexViewModel();
            viewModel.Pagina = 1;

            viewModel.CalcularPaginacion( AccesoUsuarios.Count());
            viewModel.listadoAccesoUsuarios = AccesoUsuarios.Take(viewModel.resultadosPorPagina).ToList();

            return View(viewModel);
        }




        // POST: Admin/AccesoUsuarios/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index( AccesoUsuariosIndexViewModel viewModel)
        {
            var busqueda = db.AccesoUsuarios.OrderByDescending(au => au.FechaAcceso).ToList();

            if (!string.IsNullOrWhiteSpace(viewModel.TextoBusqueda))
            {
                busqueda = busqueda.Where(x => x.Usuario.Contains(viewModel.TextoBusqueda)).ToList();
            }

            viewModel.CalcularPaginacion( busqueda.Count());

            int skip = (viewModel.Pagina - 1) * viewModel.resultadosPorPagina;
            viewModel.listadoAccesoUsuarios = busqueda.Skip(skip).Take(viewModel.resultadosPorPagina).ToList();
            return View(viewModel);
        }
        
    }

}
