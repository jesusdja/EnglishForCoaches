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
using EnglishForCoachesWeb.Database.JuegoOnline;

namespace EnglishForCoachesWeb.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BeatTheGoaliesController : MVCBaseController
    {
        private AuthContext db = new AuthContext();

        // GET: Admin/BeatTheGoalies/Create
        public ActionResult Create(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JuegoOnline JuegoOnline =db.JuegoOnlines.SingleOrDefault(bl => bl.JuegoOnlineId == id);
            if (JuegoOnline == null)
            {
                return HttpNotFound();
            }

            BeatTheGoalieCreateViewModel viewModel = new BeatTheGoalieCreateViewModel();
            viewModel.Inicializar(id);
            
            viewModel.BeatTheGoalie = new BeatTheGoalie();
            viewModel.BeatTheGoalie.TipoJuegoOnlineId = (int)TiposDeJuegosOnlineId.BeatTheGoalie;
            viewModel.BeatTheGoalie.JuegoOnlineId = id;
            viewModel.BeatTheGoalie.SubTemaId = JuegoOnline.SubTemaId;
            return View(viewModel);
        }
        
        // POST: Admin/BeatTheGoalies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BeatTheGoalieCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {

                viewModel.BeatTheGoalie.Descripcion = viewModel.BeatTheGoalie.Respuesta1 + " | " + viewModel.BeatTheGoalie.Respuesta2 + " | " + viewModel.BeatTheGoalie.Respuesta3 + " | " + viewModel.BeatTheGoalie.Respuesta4;
                if(!string.IsNullOrEmpty(viewModel.BeatTheGoalie.Enunciado))
                {
                    viewModel.BeatTheGoalie.Descripcion = viewModel.BeatTheGoalie.Enunciado;
                }
                db.BeatTheGoalies.Add(viewModel.BeatTheGoalie);
                db.SaveChanges();

                if (viewModel.AudioFile != null)
                {
                    viewModel.BeatTheGoalie.FicheroAudio = viewModel.BeatTheGoalie.Id + ".mp3";

                    string nameAndLocation = "~/media/upload/BeatTheGoalie/" + viewModel.BeatTheGoalie.FicheroAudio;
                    viewModel.AudioFile.SaveAs(Server.MapPath(nameAndLocation));

                    db.Entry(viewModel.BeatTheGoalie).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Create", "BeatTheGoalies", new { id = viewModel.BeatTheGoalie.JuegoOnlineId});
            }


            viewModel.Inicializar(viewModel.BeatTheGoalie.JuegoOnlineId);
            return View(viewModel);
        }

        // GET: Admin/BeatTheGoalies/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BeatTheGoalie BeatTheGoalie = db.BeatTheGoalies.Find(id);
            if (BeatTheGoalie == null)
            {
                return HttpNotFound();
            }

            BeatTheGoalieEditViewModel viewModel = new BeatTheGoalieEditViewModel();
            viewModel.Inicializar(BeatTheGoalie.JuegoOnlineId);

            viewModel.BeatTheGoalie = BeatTheGoalie;
            return View(viewModel);
        }

        // POST: Admin/BeatTheGoalies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BeatTheGoalieEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                viewModel.BeatTheGoalie.Descripcion = viewModel.BeatTheGoalie.Respuesta1 + " | "+ viewModel.BeatTheGoalie.Respuesta2 + " | " + viewModel.BeatTheGoalie.Respuesta3 + " | " + viewModel.BeatTheGoalie.Respuesta4 ;
                if (!string.IsNullOrEmpty(viewModel.BeatTheGoalie.Enunciado))
                {
                    viewModel.BeatTheGoalie.Descripcion = viewModel.BeatTheGoalie.Enunciado;
                }

                if (viewModel.AudioFile != null)
                {
                    if (viewModel.BeatTheGoalie.FicheroAudio != null)
                    {
                        var fullPath = Request.MapPath("~/media/upload/BeatTheGoalie/" + viewModel.BeatTheGoalie.FicheroAudio);
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }
                    }

                    viewModel.BeatTheGoalie.FicheroAudio = viewModel.BeatTheGoalie.Id + ".mp3";

                    string nameAndLocation = "~/media/upload/BeatTheGoalie/" + viewModel.BeatTheGoalie.FicheroAudio;
                    viewModel.AudioFile.SaveAs(Server.MapPath(nameAndLocation));
                }

                db.Entry(viewModel.BeatTheGoalie).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Create", "BeatTheGoalies", new { id = viewModel.BeatTheGoalie.JuegoOnlineId });
            }
            viewModel.Inicializar(viewModel.BeatTheGoalie.JuegoOnlineId);
            return View(viewModel);
        }

        // GET: Admin/BeatTheGoalies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BeatTheGoalie BeatTheGoalie = db.BeatTheGoalies.Find(id);
            if (BeatTheGoalie == null)
            {
                return HttpNotFound();
            }
            if (BeatTheGoalie.FicheroAudio != null)
            {
                var fullPath = Request.MapPath("~/media/upload/BeatTheGoalie/" + BeatTheGoalie.FicheroAudio);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }

            db.BeatTheGoalies.Remove(BeatTheGoalie);
            db.SaveChanges();
            return RedirectToAction("Create", "BeatTheGoalies", new { id = BeatTheGoalie.JuegoOnlineId });
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

