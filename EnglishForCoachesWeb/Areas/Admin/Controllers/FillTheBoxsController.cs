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
    public class FillTheBoxsController : MVCBaseController
    {
        private AuthContext db = new AuthContext();
        

        // GET: Admin/FillTheBoxs/Create
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

            FillTheBoxCreateViewModel viewModel = new FillTheBoxCreateViewModel();
            viewModel.Inicializar(id);

            viewModel.FillTheBox = new FillTheBox();
            viewModel.FillTheBox.TipoEjercicioId = (int)TiposDeEjerciciosId.FillTheBox;
            viewModel.FillTheBox.BloqueId = id;
            viewModel.FillTheBox.SubTemaId = bloque.SubTemaId;
            viewModel.FillTheBox.AreaId = viewModel.bloque.AreaId;
            return View(viewModel);
        }

        // POST: Admin/FillTheBoxs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FillTheBoxCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                viewModel.FillTheBox.Descripcion = viewModel.FillTheBox.Enunciado.Replace("#", "______");

                db.FillTheBoxs.Add(viewModel.FillTheBox);
                db.SaveChanges();

                if (viewModel.AudioFile != null)
                {
                    viewModel.FillTheBox.FicheroAudio = viewModel.FillTheBox.Id + ".mp3";

                    string nameAndLocation = "~/media/upload/audio_fillthebox/" + viewModel.FillTheBox.FicheroAudio;
                    viewModel.AudioFile.SaveAs(Server.MapPath(nameAndLocation));

                    db.Entry(viewModel.FillTheBox).State = EntityState.Modified;
                    db.SaveChanges();
                }
                if (viewModel.ImageFile != null)
                {
                    viewModel.FillTheBox.UrlImagen = viewModel.FillTheBox.Id + ".jpg";

                    string nameAndLocation = "~/media/upload/imagen_fillthebox/" + viewModel.FillTheBox.UrlImagen;
                    viewModel.ImageFile.SaveAs(Server.MapPath(nameAndLocation));


                    db.Entry(viewModel.FillTheBox).State = EntityState.Modified;
                    db.SaveChanges();
                }

                return RedirectToAction("Create", "FillTheBoxs", new { id = viewModel.FillTheBox.BloqueId });
            }


            viewModel.Inicializar(viewModel.FillTheBox.BloqueId);
            return View(viewModel);
        }

        // GET: Admin/FillTheBoxs/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FillTheBox FillTheBox = db.FillTheBoxs.Find(id);
            if (FillTheBox == null)
            {
                return HttpNotFound();
            }

            FillTheBoxEditViewModel viewModel = new FillTheBoxEditViewModel();
            viewModel.Inicializar(FillTheBox.BloqueId);

            viewModel.FillTheBox = FillTheBox;
            return View(viewModel);
        }

        // POST: Admin/FillTheBoxs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FillTheBoxEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (viewModel.AudioFile != null)
                {
                    string fullPath = Request.MapPath("~/media/upload/audio_fillthebox/" + viewModel.FillTheBox.FicheroAudio);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                    viewModel.FillTheBox.FicheroAudio = viewModel.FillTheBox.Id + ".mp3";

                    string nameAndLocation = "~/media/upload/audio_fillthebox/" + viewModel.FillTheBox.FicheroAudio;
                    viewModel.AudioFile.SaveAs(Server.MapPath(nameAndLocation));
                }

                if (viewModel.ImageFile != null)
                {
                    var imgfullPath = Request.MapPath("~/media/upload/imagen_fillthebox/" + viewModel.FillTheBox.UrlImagen);
                    if (System.IO.File.Exists(imgfullPath))
                    {
                        System.IO.File.Delete(imgfullPath);
                    }
                    viewModel.FillTheBox.UrlImagen = viewModel.FillTheBox.Id + ".jpg";

                    string nameAndLocation = "~/media/upload/imagen_fillthebox/" + viewModel.FillTheBox.UrlImagen;
                    viewModel.ImageFile.SaveAs(Server.MapPath(nameAndLocation));
                }


                viewModel.FillTheBox.Descripcion = viewModel.FillTheBox.Enunciado.Replace("#", "______");
                db.Entry(viewModel.FillTheBox).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Create", "FillTheBoxs", new { id = viewModel.FillTheBox.BloqueId });
            }
            viewModel.Inicializar(viewModel.FillTheBox.SubTemaId);
            return View(viewModel);
        }

        // GET: Admin/FillTheBoxs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FillTheBox FillTheBox = db.FillTheBoxs.Find(id);
            if (FillTheBox == null)
            {
                return HttpNotFound();
            }

            var imgfullPath = Request.MapPath("~/media/upload/imagen_fillthebox/" + FillTheBox.UrlImagen);
            if (System.IO.File.Exists(imgfullPath))
            {
                System.IO.File.Delete(imgfullPath);
            }
            string fullPath = Request.MapPath("~/media/upload/audio_fillthebox/" + FillTheBox.FicheroAudio);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
            db.FillTheBoxs.Remove(FillTheBox);
            db.SaveChanges();
            return RedirectToAction("Create", "FillTheBoxs", new { id = FillTheBox.BloqueId });
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
