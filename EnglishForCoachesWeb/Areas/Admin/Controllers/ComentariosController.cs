using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EnglishForCoachesWeb.Database;
using EnglishForCoachesWeb.Controllers;
using EnglishForCoachesWeb.Areas.Admin.Models;

namespace EnglishForCoachesWeb.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ComentariosController : MVCBaseController
    {
        private AuthContext db = new AuthContext();

        // GET: Admin/Comentarios
        public ActionResult Index()
        {
            ComentarioViewModel model = new ComentarioViewModel();
            model.Comentarios = db.Comentarios.OrderByDescending(c=>c.FechaHoraComentario).ToList();
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            Comentario comentario = db.Comentarios.Find(id);
            db.Comentarios.Remove(comentario);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Admin/Articulos/activar/0
        public ActionResult Aceptar(int id)
        {
            var comentario = db.Comentarios.Find(id);
            comentario.Aceptado = !comentario.Aceptado;
            db.Entry(comentario).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
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
