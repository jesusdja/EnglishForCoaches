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
    public class AudioSentenceExercisesController : MVCBaseController
    {
        private AuthContext db = new AuthContext();
        

        // GET: Admin/AudioSentenceExercises/Create
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

            AudioSentenceExerciseCreateViewModel viewModel = new AudioSentenceExerciseCreateViewModel();
            viewModel.Inicializar(id);

            viewModel.AudioSentenceExercise = new AudioSentenceExercise();
            viewModel.AudioSentenceExercise.TipoEjercicioId = (int)TiposDeEjerciciosId.AudioSentenceExercise;
            viewModel.AudioSentenceExercise.BloqueId = id;
            viewModel.AudioSentenceExercise.SubTemaId = bloque.SubTemaId;
            viewModel.AudioSentenceExercise.AreaId = viewModel.bloque.AreaId;
            return View(viewModel);
        }

        // POST: Admin/AudioSentenceExercises/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AudioSentenceExerciseCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                viewModel.AudioSentenceExercise.Descripcion = viewModel.AudioSentenceExercise.Enunciado.Replace("#", "______");

                db.AudioSentenceExercises.Add(viewModel.AudioSentenceExercise);
                db.SaveChanges();


                if (viewModel.AudioFile != null)
                {
                    viewModel.AudioSentenceExercise.FicheroAudio = viewModel.AudioSentenceExercise.Id + ".mp3";

                    string nameAndLocation = "~/media/upload/audio_ejercicio/" + viewModel.AudioSentenceExercise.FicheroAudio;
                    viewModel.AudioFile.SaveAs(Server.MapPath(nameAndLocation));

                    db.Entry(viewModel.AudioSentenceExercise).State = EntityState.Modified;
                    db.SaveChanges();
                }

                return RedirectToAction("Create", "AudioSentenceExercises", new { id = viewModel.AudioSentenceExercise.BloqueId });
            }


            viewModel.Inicializar(viewModel.AudioSentenceExercise.BloqueId);
            return View(viewModel);
        }

        // GET: Admin/AudioSentenceExercises/Edit/5
        public ActionResult Edit(int id, int? examenId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AudioSentenceExercise AudioSentenceExercise = db.AudioSentenceExercises.Find(id);
            if (AudioSentenceExercise == null)
            {
                return HttpNotFound();
            }

            AudioSentenceExerciseEditViewModel viewModel = new AudioSentenceExerciseEditViewModel();
            viewModel.Inicializar(AudioSentenceExercise.BloqueId);
            viewModel.ExamenId = examenId;

            viewModel.AudioSentenceExercise = AudioSentenceExercise;
            return View(viewModel);
        }

        // POST: Admin/AudioSentenceExercises/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AudioSentenceExerciseEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (viewModel.AudioFile != null)
                {
                    string fullPath = Request.MapPath("~/media/upload/audio_ejercicio/" + viewModel.AudioSentenceExercise.FicheroAudio);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                    viewModel.AudioSentenceExercise.FicheroAudio = viewModel.AudioSentenceExercise.Id + ".mp3";

                    string nameAndLocation = "~/media/upload/audio_ejercicio/" + viewModel.AudioSentenceExercise.FicheroAudio;
                    viewModel.AudioFile.SaveAs(Server.MapPath(nameAndLocation));
                }

                viewModel.AudioSentenceExercise.Descripcion = viewModel.AudioSentenceExercise.Enunciado.Replace("#", "______");
                db.Entry(viewModel.AudioSentenceExercise).State = EntityState.Modified;
                db.SaveChanges();
                if (viewModel.ExamenId.HasValue) {
                    return RedirectToAction("Edit", "ErroresExamens", new { id = viewModel.ExamenId.Value });
                } else {
                    return RedirectToAction("Create", "AudioSentenceExercises", new { id = viewModel.AudioSentenceExercise.BloqueId });
                }
            }
            viewModel.Inicializar(viewModel.AudioSentenceExercise.SubTemaId);
            return View(viewModel);
        }

        // GET: Admin/AudioSentenceExercises/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AudioSentenceExercise AudioSentenceExercise = db.AudioSentenceExercises.Find(id);
            if (AudioSentenceExercise == null)
            {
                return HttpNotFound();
            }

            string fullPath = Request.MapPath("~/media/upload/audio_ejercicio/" + AudioSentenceExercise.FicheroAudio);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
            db.AudioSentenceExercises.Remove(AudioSentenceExercise);
            db.SaveChanges();
            return RedirectToAction("Create", "AudioSentenceExercises", new { id = AudioSentenceExercise.BloqueId });
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
