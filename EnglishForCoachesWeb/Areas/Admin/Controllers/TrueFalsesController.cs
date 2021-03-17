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
    public class TrueFalsesController : MVCBaseController
    {
        private AuthContext db = new AuthContext();

        // GET: Admin/TrueFalses/Create
        public ActionResult Create(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bloque bloque =db.Bloques.SingleOrDefault(bl => bl.BloqueId == id);
            if (bloque == null)
            {
                return HttpNotFound();
            }

            TrueFalseCreateViewModel viewModel = new TrueFalseCreateViewModel();
            viewModel.Inicializar(id);
            
            viewModel.TrueFalse = new TrueFalse();
            viewModel.TrueFalse.TipoEjercicioId = (int)TiposDeEjerciciosId.TrueFalse;
            viewModel.TrueFalse.BloqueId = id;
            viewModel.TrueFalse.SubTemaId = bloque.SubTemaId;
            viewModel.TrueFalse.AreaId = viewModel.bloque.AreaId;
            return View(viewModel);
        }
        
        // POST: Admin/TrueFalses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TrueFalseCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                viewModel.TrueFalse.Descripcion = viewModel.TrueFalse.Enunciado;

                db.TrueFalses.Add(viewModel.TrueFalse);
                db.SaveChanges();

                if (viewModel.AudioFile != null)
                {
                    viewModel.TrueFalse.Audio = viewModel.TrueFalse.Id + ".mp3";

                    string nameAndLocation = "~/media/upload/TrueFalse/Audios/" + viewModel.TrueFalse.Audio;
                    viewModel.AudioFile.SaveAs(Server.MapPath(nameAndLocation));

                    db.Entry(viewModel.TrueFalse).State = EntityState.Modified;
                    db.SaveChanges();
                }
                if (viewModel.ImageFile != null)
                {
                    viewModel.TrueFalse.UrlImagen = viewModel.TrueFalse.Id + ".jpg";

                    string nameAndLocation = "~/media/upload/TrueFalse/" + viewModel.TrueFalse.UrlImagen;
                    viewModel.ImageFile.SaveAs(Server.MapPath(nameAndLocation));
             
                    db.Entry(viewModel.TrueFalse).State = EntityState.Modified;
                    db.SaveChanges();
                }

                return RedirectToAction("Create", "TrueFalses", new { id = viewModel.TrueFalse.BloqueId});
            }


            viewModel.Inicializar(viewModel.TrueFalse.BloqueId);
            return View(viewModel);
        }

        // GET: Admin/TrueFalses/Edit/5
        public ActionResult Edit(int id, int? examenId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrueFalse TrueFalse = db.TrueFalses.Find(id);
            if (TrueFalse == null)
            {
                return HttpNotFound();
            }

            TrueFalseEditViewModel viewModel = new TrueFalseEditViewModel();
            viewModel.Inicializar(TrueFalse.BloqueId);
            viewModel.ExamenId = examenId;
            viewModel.TrueFalse = TrueFalse;
            return View(viewModel);
        }

        // POST: Admin/TrueFalses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TrueFalseEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (viewModel.AudioFile != null)
                {
                    string fullPath = Request.MapPath("~/media/upload/TrueFalse/Audios/" + viewModel.TrueFalse.Audio);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                    viewModel.TrueFalse.Audio = viewModel.TrueFalse.Id + ".mp3";

                    string nameAndLocation = "~/media/upload/TrueFalse/Audios/" + viewModel.TrueFalse.Audio;
                    viewModel.AudioFile.SaveAs(Server.MapPath(nameAndLocation));
                }

                if (viewModel.ImageFile != null)
                {
                    if (viewModel.TrueFalse.UrlImagen != null)
                    {
                        var fullPath = Request.MapPath("~/media/upload/TrueFalse/" + viewModel.TrueFalse.UrlImagen);
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }
                    }

                    viewModel.TrueFalse.UrlImagen = viewModel.TrueFalse.Id + ".jpg";

                    string nameAndLocation = "~/media/upload/TrueFalse/" + viewModel.TrueFalse.UrlImagen;
                    viewModel.ImageFile.SaveAs(Server.MapPath(nameAndLocation));
                }


                viewModel.TrueFalse.Descripcion = viewModel.TrueFalse.Enunciado;
                db.Entry(viewModel.TrueFalse).State = EntityState.Modified;
                db.SaveChanges();
                if (viewModel.ExamenId.HasValue) {
                    return RedirectToAction("Edit", "ErroresExamens", new { id = viewModel.ExamenId.Value });
                } else {
                    return RedirectToAction("Create", "TrueFalses", new { id = viewModel.TrueFalse.BloqueId });
                }
            }
            viewModel.Inicializar(viewModel.TrueFalse.BloqueId);
            return View(viewModel);
        }

        // GET: Admin/TrueFalses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrueFalse TrueFalse = db.TrueFalses.Find(id);
            if (TrueFalse == null)
            {
                return HttpNotFound();
            }
            if (TrueFalse.UrlImagen != null)
            {
                string fullPath = Request.MapPath("~/media/upload/TrueFalse/Audios/" + TrueFalse.Audio);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
                fullPath = Request.MapPath("~/media/upload/TrueFalse/" + TrueFalse.UrlImagen);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }

            db.TrueFalses.Remove(TrueFalse);
            db.SaveChanges();
            return RedirectToAction("Create", "TrueFalses", new { id = TrueFalse.BloqueId });
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

