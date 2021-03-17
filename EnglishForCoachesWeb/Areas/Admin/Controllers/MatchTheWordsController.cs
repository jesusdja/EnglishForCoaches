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
    public class MatchTheWordsController : MVCBaseController
    {
        private AuthContext db = new AuthContext();


        // GET: Admin/MatchTheWords/Create
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
            MatchTheWordCreateViewModel viewModel = new MatchTheWordCreateViewModel();
            viewModel.Inicializar(id);
            viewModel.MatchTheWord = new MatchTheWord();
            viewModel.MatchTheWord.TipoEjercicioId =(int) TiposDeEjerciciosId.MatchTheWord;
            viewModel.MatchTheWord.BloqueId = id;
            viewModel.MatchTheWord.SubTemaId = bloque.SubTemaId;
            viewModel.MatchTheWord.AreaId = viewModel.bloque.AreaId;
            return View(viewModel);
        }

        // POST: Admin/MatchTheWords/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MatchTheWordCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                viewModel.MatchTheWord.Descripcion = viewModel.MatchTheWord.Pregunta + " / "+ viewModel.MatchTheWord.Respuesta;

                db.MatchTheWords.Add(viewModel.MatchTheWord);
                db.SaveChanges();
                return RedirectToAction("Create", "MatchTheWords", new { id = viewModel.MatchTheWord.BloqueId });
            }


            viewModel.Inicializar(viewModel.MatchTheWord.BloqueId);
            return View(viewModel);
        }

        // GET: Admin/MatchTheWords/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MatchTheWord matchTheWord = db.MatchTheWords.Find(id);
            if (matchTheWord == null)
            {
                return HttpNotFound();
            }


            MatchTheWordEditViewModel viewModel = new MatchTheWordEditViewModel();
            viewModel.Inicializar(matchTheWord.BloqueId);
            viewModel.MatchTheWord = matchTheWord;
            return View(viewModel);
        }

        // POST: Admin/MatchTheWords/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MatchTheWordEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                viewModel.MatchTheWord.Descripcion = viewModel.MatchTheWord.Pregunta + " / " + viewModel.MatchTheWord.Respuesta;
                db.Entry(viewModel.MatchTheWord).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Create", "MatchTheWords", new { id = viewModel.MatchTheWord.BloqueId });
            }
            viewModel.Inicializar(viewModel.MatchTheWord.SubTemaId);
            return View(viewModel);
        }

        // GET: Admin/MatchTheWords/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MatchTheWord matchTheWord = db.MatchTheWords.Find(id);
            if (matchTheWord == null)
            {
                return HttpNotFound();
            }
        
            db.MatchTheWords.Remove(matchTheWord);
            db.SaveChanges();
            return RedirectToAction("Create", "MatchTheWords", new { id = matchTheWord.BloqueId });
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
