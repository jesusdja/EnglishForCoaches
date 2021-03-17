using EnglishForCoachesWeb.Areas.Publico.Models;
using EnglishForCoachesWeb.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace EnglishForCoachesWeb.Areas.Publico.Controllers
{
    public class ErrorController : Controller
    {
        private AuthContext db = new AuthContext();

        public ActionResult NotFound()
        {
            var fechaActual = DateTime.Now;
            BuscarViewModel model = new BuscarViewModel();

            model.ArticulosRecientes = db.Articulos.Include(a => a.Categoria).Where(art => art.Publicado && art.FechaPublicacion <= fechaActual)
                .OrderByDescending(art => art.FechaPublicacion).Take(6).ToList();

            Response.StatusCode = 404;
            return View(model);
        }
        public ActionResult Error500()
        {
            var fechaActual = DateTime.Now;
            BuscarViewModel model = new BuscarViewModel();

            model.ArticulosRecientes = db.Articulos.Include(a => a.Categoria).Where(art => art.Publicado && art.FechaPublicacion <= fechaActual)
                .OrderByDescending(art => art.FechaPublicacion).Take(6).ToList();

            Response.StatusCode = 500;
            return View(model);
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