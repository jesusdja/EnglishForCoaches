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
    public class WordByWordsController : MVCBaseController
    {
        private AuthContext db = new AuthContext();



        // GET: Admin/WordByWords/Create
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
            WordByWordCreateViewModel viewModel = new WordByWordCreateViewModel();
            viewModel.Inicializar(id);
            viewModel.WordByWord = new WordByWord();
            viewModel.WordByWord.TipoEjercicioId = (int)TiposDeEjerciciosId.WordByWord;
            viewModel.WordByWord.BloqueId = id;
            viewModel.WordByWord.SubTemaId = bloque.SubTemaId;
            viewModel.WordByWord.AreaId = viewModel.bloque.AreaId;
            return View(viewModel);            
        }

        // POST: Admin/WordByWords/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(WordByWordCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                viewModel.WordByWord.Descripcion = viewModel.WordByWord.Enunciado.Replace("#"," ");

                db.WordByWords.Add(viewModel.WordByWord);
                db.SaveChanges();
                return RedirectToAction("Create", "WordByWords", new { id = viewModel.WordByWord.BloqueId });
            }


            viewModel.Inicializar(viewModel.WordByWord.BloqueId);
            return View(viewModel);
        }

        // GET: Admin/WordByWords/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WordByWord WordByWord = db.WordByWords.Find(id);
            if (WordByWord == null)
            {
                return HttpNotFound();
            }


            WordByWordEditViewModel viewModel = new WordByWordEditViewModel();
            viewModel.Inicializar(WordByWord.BloqueId);
            viewModel.WordByWord = WordByWord;
            return View(viewModel);
        }

        // POST: Admin/WordByWords/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(WordByWordEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                viewModel.WordByWord.Descripcion = viewModel.WordByWord.Enunciado.Replace("#", " ");
                db.Entry(viewModel.WordByWord).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Create", "WordByWords", new { id = viewModel.WordByWord.BloqueId });
            }
            viewModel.Inicializar(viewModel.WordByWord.SubTemaId);
            return View(viewModel);
        }

        // GET: Admin/WordByWords/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WordByWord wordByWord = db.WordByWords.Find(id);
            if (wordByWord == null)
            {
                return HttpNotFound();
            }
      
            db.WordByWords.Remove(wordByWord);
            db.SaveChanges();
            return RedirectToAction("Create", "WordByWords", new { id = wordByWord.BloqueId });
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
