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
    public class FillTheGapsController : MVCBaseController
    {
        private AuthContext db = new AuthContext();
        

        // GET: Admin/FillTheGaps/Create
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

            FillTheGapCreateViewModel viewModel = new FillTheGapCreateViewModel();
            viewModel.Inicializar(id);

            viewModel.FillTheGap = new FillTheGap();
            viewModel.FillTheGap.TipoEjercicioId = (int)TiposDeEjerciciosId.FillTheGap;
            viewModel.FillTheGap.BloqueId = id;
            viewModel.FillTheGap.SubTemaId = bloque.SubTemaId;
            viewModel.FillTheGap.AreaId = viewModel.bloque.AreaId;
            return View(viewModel);
        }

        // POST: Admin/FillTheGaps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FillTheGapCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                viewModel.FillTheGap.Descripcion = viewModel.FillTheGap.Enunciado.Replace("#", "______");

                db.FillTheGaps.Add(viewModel.FillTheGap);
                db.SaveChanges();
                return RedirectToAction("Create", "FillTheGaps", new { id = viewModel.FillTheGap.BloqueId });
            }


            viewModel.Inicializar(viewModel.FillTheGap.BloqueId);
            return View(viewModel);
        }

        // GET: Admin/FillTheGaps/Edit/5
        public ActionResult Edit(int id, int? examenId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FillTheGap FillTheGap = db.FillTheGaps.Find(id);
            if (FillTheGap == null)
            {
                return HttpNotFound();
            }

            FillTheGapEditViewModel viewModel = new FillTheGapEditViewModel();
            viewModel.Inicializar(FillTheGap.BloqueId);
            viewModel.ExamenId = examenId;
            viewModel.FillTheGap = FillTheGap;
            return View(viewModel);
        }

        // POST: Admin/FillTheGaps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FillTheGapEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                viewModel.FillTheGap.Descripcion = viewModel.FillTheGap.Enunciado.Replace("#", "______");
                db.Entry(viewModel.FillTheGap).State = EntityState.Modified;
                db.SaveChanges();
                if (viewModel.ExamenId.HasValue) {
                    return RedirectToAction("Edit", "ErroresExamens", new { id = viewModel.ExamenId.Value });
                } else {
                    return RedirectToAction("Create", "FillTheGaps", new { id = viewModel.FillTheGap.BloqueId });
                }
            }
            viewModel.Inicializar(viewModel.FillTheGap.SubTemaId);
            return View(viewModel);
        }

        // GET: Admin/FillTheGaps/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FillTheGap FillTheGap = db.FillTheGaps.Find(id);
            if (FillTheGap == null)
            {
                return HttpNotFound();
            }

            db.FillTheGaps.Remove(FillTheGap);
            db.SaveChanges();
            return RedirectToAction("Create", "FillTheGaps", new { id = FillTheGap.BloqueId });
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
