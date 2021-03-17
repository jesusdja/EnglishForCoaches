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
using static EnglishForCoachesWeb.Areas.Admin.Controllers.FrasesController;

namespace EnglishForCoachesWeb.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AhorcadoController : MVCBaseController
    {
        private AuthContext db = new AuthContext();



        // GET: Admin/Ahorcado/Create
        public ActionResult Create(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JuegoOnline JuegoOnline = db.JuegoOnlines.SingleOrDefault(bl => bl.JuegoOnlineId == id);
            if (JuegoOnline == null)
            {
                return HttpNotFound();
            }

            AhorcadoCreateViewModel viewModel = new AhorcadoCreateViewModel();
            viewModel.Inicializar(id);

            viewModel.Ahorcado = new Ahorcado();
            viewModel.Ahorcado.TipoJuegoOnlineId = (int)TiposDeJuegosOnlineId.Ahorcado;
            viewModel.Ahorcado.JuegoOnlineId = id;
            viewModel.Ahorcado.SubTemaId = JuegoOnline.SubTemaId;
            return View(viewModel);
        }

        // POST: Admin/Ahorcado/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AhorcadoCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {

                viewModel.Ahorcado.Descripcion = viewModel.Ahorcado.Texto ;           
                db.Ahorcado.Add(viewModel.Ahorcado);
                db.SaveChanges();

                if (viewModel.AudioFile != null)
                {
                    viewModel.Ahorcado.Audio = viewModel.Ahorcado.Id + ".mp3";

                    string nameAndLocation = "~/media/upload/Ahorcado/Audios/" + viewModel.Ahorcado.Audio;
                    viewModel.AudioFile.SaveAs(Server.MapPath(nameAndLocation));

                    db.Entry(viewModel.Ahorcado).State = EntityState.Modified;
                    db.SaveChanges();
                }
                if (viewModel.ImageFile != null)
                {
                    viewModel.Ahorcado.UrlImagen = viewModel.Ahorcado.Id + ".jpg";

                    string nameAndLocation = "~/media/upload/Ahorcado/" + viewModel.Ahorcado.UrlImagen;
                    viewModel.ImageFile.SaveAs(Server.MapPath(nameAndLocation));

                    db.Entry(viewModel.Ahorcado).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Create", "Ahorcado", new { id = viewModel.Ahorcado.JuegoOnlineId });
            }


            viewModel.Inicializar(viewModel.Ahorcado.JuegoOnlineId);
            return View(viewModel);
        }





        // GET: Admin/Ahorcado/Edit
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ahorcado Ahorcado = db.Ahorcado.Find(id);
            if (Ahorcado == null)
            {
                return HttpNotFound();
            }
            AhorcadoEditViewModel viewModel = new AhorcadoEditViewModel();
            viewModel.Inicializar(Ahorcado.JuegoOnlineId);
            viewModel.Ahorcado = Ahorcado;          

            return View(viewModel);
        }
        // POST: Admin/Ahorcado/Edit
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AhorcadoEditViewModel viewModel)
        {           
            if (ModelState.IsValid)
            {
                viewModel.Ahorcado.Descripcion = viewModel.Ahorcado.Texto;
                
                db.Entry(viewModel.Ahorcado).State = EntityState.Modified;
                db.SaveChanges();


                if (viewModel.AudioFile != null)
                {
                    viewModel.Ahorcado.Audio = viewModel.Ahorcado.Id + ".mp3";

                    string nameAndLocation = "~/media/upload/Ahorcado/Audios/" + viewModel.Ahorcado.Audio;
                    viewModel.AudioFile.SaveAs(Server.MapPath(nameAndLocation));

                    db.Entry(viewModel.Ahorcado).State = EntityState.Modified;
                    db.SaveChanges();
                }
                if (viewModel.ImageFile != null)
                {
                    viewModel.Ahorcado.UrlImagen = viewModel.Ahorcado.Id + ".jpg";

                    string nameAndLocation = "~/media/upload/Ahorcado/" + viewModel.Ahorcado.UrlImagen;
                    viewModel.ImageFile.SaveAs(Server.MapPath(nameAndLocation));

                    db.Entry(viewModel.Ahorcado).State = EntityState.Modified;
                    db.SaveChanges();
                }


                return RedirectToAction("Create", "Ahorcado", new { id = viewModel.Ahorcado.JuegoOnlineId });
            }
            viewModel.Ahorcado = db.Ahorcado.FirstOrDefault(c => c.JuegoOnlineId == viewModel.Ahorcado.JuegoOnlineId);
            viewModel.Inicializar(viewModel.Ahorcado.SubTemaId);
            return View(viewModel);
        }



        // GET: Admin/Ahorcado/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ahorcado Ahorcado = db.Ahorcado.Find(id);
            if (Ahorcado == null)
            {
                return HttpNotFound();
            }
            if (Ahorcado.UrlImagen != null)
            {

                string fullPath = Request.MapPath("~/media/upload/Ahorcado/" + Ahorcado.UrlImagen);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }
            if (Ahorcado.Audio != null)
            {
                string fullPath = Request.MapPath("~/media/upload/Ahorcado/Audios/" + Ahorcado.Audio);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }

            db.Ahorcado.Remove(Ahorcado);
            db.SaveChanges();
            return RedirectToAction("Create", "Ahorcado", new { id = Ahorcado.JuegoOnlineId });
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
