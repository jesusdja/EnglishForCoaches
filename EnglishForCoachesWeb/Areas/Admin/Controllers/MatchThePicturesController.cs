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
    public class MatchThePicturesController : MVCBaseController
    {
        private AuthContext db = new AuthContext();


        // GET: Admin/MatchThePictures/Create
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
            MatchThePictureCreateViewModel viewModel = new MatchThePictureCreateViewModel();
            viewModel.Inicializar(id);
            viewModel.MatchThePicture = new MatchThePicture();
            viewModel.MatchThePicture.TipoEjercicioId =(int) TiposDeEjerciciosId.MatchThePicture;
            viewModel.MatchThePicture.BloqueId = id;
            viewModel.MatchThePicture.SubTemaId = bloque.SubTemaId;
            viewModel.MatchThePicture.AreaId = viewModel.bloque.AreaId;
            return View(viewModel);
        }

        // POST: Admin/MatchThePictures/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MatchThePictureCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                viewModel.MatchThePicture.Descripcion = viewModel.MatchThePicture.PalabraImagen;

                db.MatchThePictures.Add(viewModel.MatchThePicture);
                db.SaveChanges();



                viewModel.MatchThePicture.UrlImagen = viewModel.MatchThePicture.Id + ".jpg";

                string nameAndLocation = "~/media/upload/MatchThePicture/" + viewModel.MatchThePicture.UrlImagen;
                viewModel.ImageFile.SaveAs(Server.MapPath(nameAndLocation));

                db.Entry(viewModel.MatchThePicture).State = EntityState.Modified;
                db.SaveChanges();


                return RedirectToAction("Create", "MatchThePictures", new { id = viewModel.MatchThePicture.BloqueId });
            }


            viewModel.Inicializar(viewModel.MatchThePicture.BloqueId);
            return View(viewModel);
        }
        
        // GET: Admin/MatchThePictures/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MatchThePicture matchThePicture = db.MatchThePictures.Find(id);
            if (matchThePicture == null)
            {
                return HttpNotFound();
            }
            var fullPath = Request.MapPath("~/media/upload/MatchThePicture/" + matchThePicture.UrlImagen);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }

            db.MatchThePictures.Remove(matchThePicture);
            db.SaveChanges();
            return RedirectToAction("Create", "MatchThePictures", new { id = matchThePicture.BloqueId });
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
