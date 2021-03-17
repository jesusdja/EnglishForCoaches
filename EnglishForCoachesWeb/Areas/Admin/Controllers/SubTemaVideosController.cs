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
using EnglishForCoachesWeb.Helpers;

namespace EnglishForCoachesWeb.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SubTemaVideosController : MVCBaseController
    {
        private AuthContext db = new AuthContext();


        // GET: Admin/SubTemaVideos/Create
        public ActionResult Create(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var subtema = db.SubTemas.Include(s => s.Tema).SingleOrDefault(s => s.SubTemaId == id);
            if (subtema == null)
            {
                return HttpNotFound();
            }

            SubTemaVideoCreateViewModel viewModel = new SubTemaVideoCreateViewModel();
            viewModel.Subtema = subtema;
            viewModel.CargarClienteSeleccionado(db);

            return View(viewModel);
        }

        // POST: Admin/SubTemaVideos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SubTemaVideoCreateViewModel viewModel)
        {
            //fichero pdf
            //"~/media/upload/SubTemaVideos_puntos/"

            if (ModelState.IsValid)
            {

                viewModel.SubTemaVideo.SubTemaId = viewModel.Subtema.SubTemaId;
                db.SubTemaVideos.Add(viewModel.SubTemaVideo);
                db.SaveChanges();
                AccesoClientesHelper.AnyadirSubTemaVideoConHijos(viewModel.SubTemaVideo.SubTemaVideoId, viewModel.Clientes.Where(cli => cli.Selected)
                    .Select(cli => Convert.ToInt32(cli.Value)).ToList());

                return RedirectToAction("Index", "Bloques", new { id = viewModel.SubTemaVideo.SubTemaId, pestanya = (int)PestanyasBloques.Videos });
            }
            return View(viewModel);
        }

        // GET: Admin/SubTemaVideos/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubTemaVideo SubTemaVideo = db.SubTemaVideos.FirstOrDefault(a => a.SubTemaVideoId == id);
            if (SubTemaVideo == null)
            {
                return HttpNotFound();
            }


            SubTemaVideoEditViewModel viewModel = new SubTemaVideoEditViewModel();
            var subtema = db.SubTemas.Include(s => s.Tema).SingleOrDefault(s => s.SubTemaId == SubTemaVideo.SubTemaId);
            viewModel.Subtema = subtema;

            viewModel.SubTemaVideo = SubTemaVideo;
            viewModel.CargarClienteSeleccionado(db);
            return View(viewModel);
        }

        // POST: Admin/SubTemaVideos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SubTemaVideoEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {

                db.Entry(viewModel.SubTemaVideo).State = EntityState.Modified;
                db.SaveChanges();
                AccesoClientesHelper.AnyadirSubTemaVideoConHijos(viewModel.SubTemaVideo.SubTemaVideoId, viewModel.Clientes.Where(cli => cli.Selected)
                    .Select(cli => Convert.ToInt32(cli.Value)).ToList());
                return RedirectToAction("Index", "Bloques", new { id = viewModel.SubTemaVideo.SubTemaId, pestanya = (int)PestanyasBloques.Videos });
            }

            var subtema = db.SubTemas.Include(s => s.Tema).SingleOrDefault(s => s.SubTemaId == viewModel.SubTemaVideo.SubTemaId);
            viewModel.Subtema = subtema;
            return View(viewModel);
        }

        // GET: Admin/SubTemaVideos/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubTemaVideo SubTemaVideo = db.SubTemaVideos.Find(id);
            if (SubTemaVideo == null)
            {
                return HttpNotFound();
            }


            //Eliminar subtema
            db.SubTemaVideos.Remove(SubTemaVideo);
            db.SaveChanges();
            return RedirectToAction("Index", "Bloques", new { id = SubTemaVideo.SubTemaId, pestanya = (int)PestanyasBloques.Videos });
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
