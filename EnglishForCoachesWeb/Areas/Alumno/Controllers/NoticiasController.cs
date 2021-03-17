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
    public class NoticiasController : MVCBaseController
    {
        private AuthContext db = new AuthContext();

        // GET: Alumno/Noticias
        public ActionResult Index()
        {
            NoticiasIndexViewModel viewModel = new NoticiasIndexViewModel();

            viewModel.Pagina = 1;
        
                viewModel.noticias = NoticiaDataAccess.ObtenerNoticia().OrderByDescending(not => not.Fecha).ToList();
            
            viewModel.CalcularPaginacion(viewModel.noticias.Count);
            viewModel.noticias = viewModel.noticias.Take(viewModel.resultadosPorPagina).ToList();
            return View(viewModel);
        }


        // POST: Admin/Noticias/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(NoticiasIndexViewModel viewModel)
        {
            var GrupoUsuarioID = ((ClaimsIdentity)User.Identity).FindFirst("GrupoUsuario").Value;
            if (GrupoUsuarioID == "")
            {
                viewModel.noticias = NoticiaDataAccess.ObtenerNoticia().OrderByDescending(not => not.Fecha).ToList();
            }
            else
            {
                viewModel.noticias = db.NoticiaGrupos.Where(ng => ng.GrupoUsuarioId.ToString() == GrupoUsuarioID)
                    .Select(gu => gu.Noticia).OrderByDescending(not => not.Fecha).ToList();
            }


            viewModel.CalcularPaginacion(viewModel.noticias.Count());

            int skip = (viewModel.Pagina - 1) * viewModel.resultadosPorPagina;
            viewModel.noticias = viewModel.noticias.Skip(skip).Take(viewModel.resultadosPorPagina).ToList();
            return View(viewModel);
        }

        // GET: Alumno/View/id
        public ActionResult View(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var Noticia = NoticiaDataAccess.ObtenerNoticia().FirstOrDefault(b => b.NoticiaId == id);

            if (Noticia== null)
            {
                return HttpNotFound();
            }

            NoticiasViewViewModel viewModel = new NoticiasViewViewModel();
            viewModel.Noticia = Noticia;
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
