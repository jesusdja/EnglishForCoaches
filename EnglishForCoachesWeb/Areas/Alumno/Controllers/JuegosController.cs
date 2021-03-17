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
using EnglishForCoachesWeb.DataAccess;

namespace EnglishForCoachesWeb.Areas.Alumno.Controllers
{
    [Authorize(Roles = "Alumno,Admin")]
    public class JuegosController : MVCBaseController
    {
        private AuthContext db = new AuthContext();

        // GET: Alumno/Juegos/Seleccion
        public ActionResult Seleccion(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var subtema = SubTemaDataAccess.ObtenerSubTemas(db).Include(ex => ex.Tema).FirstOrDefault(b => b.SubTemaId == id);

            if (subtema == null)
            {
                return HttpNotFound();
            }
            JuegosIndexViewModel viewModel = new JuegosIndexViewModel();
            viewModel.SubTema = subtema;
            return View(viewModel);
        }

        // GET: Alumno/Juegos
        public ActionResult Index(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var subtema = SubTemaDataAccess.ObtenerSubTemas(db).Include(ex => ex.Tema).FirstOrDefault(b => b.SubTemaId == id);

            if (subtema == null)
            {
                return HttpNotFound();
            }
            var categorias = JuegoDataAccess.ObtenerJuegos(db).Include(ex => ex.CategoriaJuego).Where(b => b.SubTemaId == id).Select(b=>b.CategoriaJuego).Distinct().ToList();
            JuegosIndexViewModel viewModel = new JuegosIndexViewModel();
            viewModel.SubTema = subtema;
            viewModel.CategoriaJuegos = categorias;
            return View(viewModel);
        }

        // GET: Alumno/Juegos/Categoria
        public ActionResult Categoria(int id,int subtemaId) {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var subtema = SubTemaDataAccess.ObtenerSubTemas(db).Include(ex => ex.Tema).FirstOrDefault(b => b.SubTemaId == subtemaId);

            if (subtema == null)
            {
                return HttpNotFound();
            }
            var categoriaJuego = db.CategoriaJuegos.FirstOrDefault(b => b.CategoriaJuegoId == id);

            if (categoriaJuego == null)
            {
                return HttpNotFound();
            }
            var juegos=JuegoDataAccess.ObtenerJuegos(db).Include(ex => ex.CategoriaJuego).Where(b => b.SubTemaId == subtemaId && b.CategoriaJuegoId==id).ToList();
            JuegosCategoriaViewModel viewModel = new JuegosCategoriaViewModel();
            viewModel.SubTema = subtema;
            viewModel.Juegos = juegos;
            viewModel.CategoriaJuego = categoriaJuego;
            return View(viewModel);
        }

    

        // GET: Alumno/View/id
        public ActionResult View(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var Juego = JuegoDataAccess.ObtenerJuegos(db).Include(ex=>ex.CategoriaJuego).FirstOrDefault(b => b.JuegoId == id);

            if (Juego== null)
            {
                return HttpNotFound();
            }
            var subtema = SubTemaDataAccess.ObtenerSubTemas(db).Include(ex => ex.Tema).FirstOrDefault(b => b.SubTemaId == Juego.SubTemaId);

            JuegosViewViewModel viewModel = new JuegosViewViewModel();
            viewModel.Juego = Juego;
            viewModel.SubTema = subtema;
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
