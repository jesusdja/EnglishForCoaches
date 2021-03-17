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
using EnglishForCoachesWeb.Areas.Alumno.Models;
using EnglishForCoachesWeb.Controllers;
using EnglishForCoachesWeb.Auth;
using System.Security.Claims;
using EnglishForCoachesWeb.Helpers;

namespace EnglishForCoachesWeb.Areas.Alumno.Controllers
{
    [Authorize(Roles = "Alumno,Admin")]
    public class MistakesController : MVCBaseController
    {
        private AuthContext db = new AuthContext();

        // GET: Alumno/Mistakes
        public ActionResult Index()
        {
            MistakesIndexViewModel viewModel = new MistakesIndexViewModel();

            viewModel.Pagina = 1;
        
                viewModel.mistakes = ContenidoHelper.ObtenerMistakes().OrderByDescending(not => not.MistakeId).ToList();

            


            viewModel.CalcularPaginacion(viewModel.mistakes.Count);
            viewModel.mistakes = viewModel.mistakes.Take(viewModel.resultadosPorPagina).ToList();
            viewModel.preguntas = ContenidoHelper.ObtenerPreguntasMistakes(viewModel.mistakes);
            return View(viewModel);
        }


        // POST: Admin/Mistakes/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(MistakesIndexViewModel viewModel)
        {
            viewModel.mistakes = ContenidoHelper.ObtenerMistakes().OrderByDescending(not => not.MistakeId).ToList();



            viewModel.CalcularPaginacion(viewModel.mistakes.Count());

            int skip = (viewModel.Pagina - 1) * viewModel.resultadosPorPagina;
            viewModel.mistakes = viewModel.mistakes.Skip(skip).Take(viewModel.resultadosPorPagina).ToList();
            viewModel.preguntas = ContenidoHelper.ObtenerPreguntasMistakes(viewModel.mistakes);
            return View(viewModel);
        }

        // GET: Alumno/View/id
        public ActionResult View(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var Mistake = db.Mistakes.FirstOrDefault(b => b.MistakeId == id);

            if (Mistake== null)
            {
                return HttpNotFound();
            }

            MistakesViewViewModel viewModel = new MistakesViewViewModel();
            viewModel.Mistake = Mistake;
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
