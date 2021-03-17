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
using EnglishForCoachesWeb.Helpers;
using EnglishForCoachesWeb.Database.Varios;

namespace EnglishForCoachesWeb.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ErroresExamensController : MVCBaseController
    {
        private AuthContext db = new AuthContext();


        // GET: Admin/ErroresExamens
        public ActionResult Index()
        {
            ErroresExamenIndexViewModel viewModel = new ErroresExamenIndexViewModel();
            var busqueda = db.Examenes.OrderByDescending(not=>not.FechaExamen).ToList();
            viewModel.Pagina = 1;

            viewModel.CalcularPaginacion( busqueda.Count());
            viewModel.listadoExamenes = busqueda.Take(viewModel.resultadosPorPagina).ToList();
            return View(viewModel);
        }



        // POST: Admin/ErroresExamens/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(ErroresExamenIndexViewModel viewModel)
        {
            var busqueda = db.Examenes.OrderByDescending(not => not.FechaExamen).ToList();

            if (!string.IsNullOrWhiteSpace(viewModel.TextoBusqueda))
            {
                busqueda = busqueda.Where(x => x.NombreAlumno.Contains(viewModel.TextoBusqueda)).ToList();
            }

            viewModel.CalcularPaginacion( busqueda.Count());

            int skip = (viewModel.Pagina - 1) * viewModel.resultadosPorPagina;
            viewModel.listadoExamenes = busqueda.Skip(skip).Take(viewModel.resultadosPorPagina).ToList();
            return View(viewModel);
        }

       

        // GET: Admin/ErroresExamens/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Examen examen = db.Examenes.Include(bl => bl.SubTema.Tema).Include(ex=>ex.RespuestasIncorrectas).FirstOrDefault(a => a.ExamenId == id);
            if (examen == null)
            {
                return HttpNotFound();
            }


            ErroresExamenEditViewModel viewModel = new ErroresExamenEditViewModel();
            
            viewModel.Examen = examen;

            return View(viewModel);
        }

    

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        
    }
}
