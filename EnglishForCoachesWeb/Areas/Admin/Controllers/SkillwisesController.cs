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
    public class SkillwisesController : MVCBaseController
    {
        private AuthContext db = new AuthContext();

        // GET: Admin/Skillwises/Create
        public ActionResult Create(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bloque bloque = db.Bloques.SingleOrDefault(bl => bl.BloqueId == id);
            if (bloque == null)
            {
                return HttpNotFound();
            }
            SkillwiseCreateViewModel viewModel = new SkillwiseCreateViewModel();
            viewModel.Inicializar(id);

            viewModel.Skillwise = new Skillwise();
            viewModel.Skillwise.TipoEjercicioId = (int)TiposDeEjerciciosId.Skillwise;
            viewModel.Skillwise.BloqueId = id;
            viewModel.Skillwise.SubTemaId = bloque.SubTemaId;
            viewModel.Skillwise.AreaId = viewModel.bloque.AreaId;
            return View(viewModel);
        }
        // POST: Admin/Skillwises/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SkillwiseCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                viewModel.Skillwise.Descripcion = viewModel.Skillwise.Enunciado;

                db.Skillwises.Add(viewModel.Skillwise);
                db.SaveChanges();
                return RedirectToAction("Create", "Skillwises", new { id = viewModel.Skillwise.BloqueId });
            }


            viewModel.Inicializar(viewModel.Skillwise.BloqueId);
            return View(viewModel);
        }

        // GET: Admin/Skillwises/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Skillwise Skillwise = db.Skillwises.Find(id);
            if (Skillwise == null)
            {
                return HttpNotFound();
            }

            SkillwiseEditViewModel viewModel = new SkillwiseEditViewModel();
            viewModel.Inicializar(Skillwise.BloqueId);

            viewModel.Skillwise = Skillwise;
            return View(viewModel);
        }

        // POST: Admin/Skillwises/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SkillwiseEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                viewModel.Skillwise.Descripcion = viewModel.Skillwise.Enunciado;
                db.Entry(viewModel.Skillwise).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Create", "Skillwises", new { id = viewModel.Skillwise.BloqueId });
            }
            viewModel.Inicializar(viewModel.Skillwise.SubTemaId);
            return View(viewModel);
        }


        // GET: Admin/Skillwises/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Skillwise Skillwise = db.Skillwises.Find(id);
            if (Skillwise == null)
            {
                return HttpNotFound();
            }

            db.Skillwises.Remove(Skillwise);
            db.SaveChanges();
            return RedirectToAction("Create", "Skillwises", new { id = Skillwise.BloqueId });
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
