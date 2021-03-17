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

namespace EnglishForCoachesWeb.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class JuegosController : MVCBaseController
    {
        private AuthContext db = new AuthContext();
        

        // GET: Admin/Juegos/Create
        public ActionResult Create(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var subtema = db.SubTemas.Include(s => s.Tema).SingleOrDefault(s => s.SubTemaId == id);
            if (subtema == null)
            {
                return HttpNotFound();
            }

            JuegoCreateViewModel viewModel = new JuegoCreateViewModel();
            viewModel.Subtema = subtema;
            viewModel.InicializarDesplegables();
            viewModel.CargarClienteSeleccionado(db);
            return View(viewModel);
        }

        // POST: Admin/Juegos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(JuegoCreateViewModel viewModel)
        {
            //fichero pdf
            //"~/media/upload/Juegos_puntos/"

            if (ModelState.IsValid)
            {

                viewModel.Juego.SubTemaId = viewModel.Subtema.SubTemaId;
                db.Juegos.Add(viewModel.Juego);
                db.SaveChanges();
                if (viewModel.File != null)
                {
                    viewModel.Juego.Fichero = viewModel.Juego.JuegoId + ".pdf";

                    string nameAndLocation = "~/media/upload/juegos/" + viewModel.Juego.Fichero;
                    viewModel.File.SaveAs(Server.MapPath(nameAndLocation));

                    db.Entry(viewModel.Juego).State = EntityState.Modified;
                    db.SaveChanges();

                }
                AccesoClientesHelper.AnyadirJuegoConHijos(viewModel.Juego.JuegoId, viewModel.Clientes.Where(cli => cli.Selected)
                    .Select(cli => Convert.ToInt32(cli.Value)).ToList());
                return RedirectToAction("Index", "Bloques", new { id = viewModel.Juego.SubTemaId, pestanya = (int)PestanyasBloques.JuegosOffline });
            }
            viewModel.InicializarDesplegables();
            return View(viewModel);
        }

        // GET: Admin/Juegos/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Juego Juego = db.Juegos.FirstOrDefault(a => a.JuegoId == id);
            if (Juego == null)
            {
                return HttpNotFound();
            }


            JuegoEditViewModel viewModel = new JuegoEditViewModel();
            
            var subtema = db.SubTemas.Include(s => s.Tema).SingleOrDefault(s => s.SubTemaId == Juego.SubTemaId);
            viewModel.Subtema = subtema;
            viewModel.InicializarDesplegables();
            viewModel.Juego = Juego;
            viewModel.CargarClienteSeleccionado(db);
            return View(viewModel);
        }

        // POST: Admin/Juegos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(JuegoEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (viewModel.File != null)
                {
                    string fullPath = Request.MapPath("~/media/upload/juegos/" + viewModel.Juego.Fichero);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                    viewModel.Juego.Fichero = viewModel.Juego.JuegoId + ".pdf";


                    string nameAndLocation = "~/media/upload/juegos/" + viewModel.Juego.Fichero;
                    viewModel.File.SaveAs(Server.MapPath(nameAndLocation));
                }
                db.Entry(viewModel.Juego).State = EntityState.Modified;
                db.SaveChanges();
                AccesoClientesHelper.AnyadirJuegoConHijos(viewModel.Juego.JuegoId, viewModel.Clientes.Where(cli => cli.Selected)
                    .Select(cli => Convert.ToInt32(cli.Value)).ToList());
                return RedirectToAction("Index", "Bloques", new { id = viewModel.Juego.SubTemaId, pestanya = (int)PestanyasBloques.JuegosOffline });
            }

            viewModel.InicializarDesplegables();
            return View(viewModel);
        }

        // GET: Admin/Juegos/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Juego Juego = db.Juegos.Find(id);
            if (Juego == null)
            {
                return HttpNotFound();
            }
                 

            string fullPath = Request.MapPath("~/media/upload/juegos/" + Juego.Fichero);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
           
            //Eliminar subtema
            db.Juegos.Remove(Juego);
            db.SaveChanges();
            return RedirectToAction("Index", "Bloques", new { id = Juego.SubTemaId, pestanya = (int)PestanyasBloques.JuegosOffline });
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
