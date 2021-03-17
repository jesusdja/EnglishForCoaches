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
using EnglishForCoachesWeb.Helpers;
using System.Drawing.Imaging;

namespace EnglishForCoachesWeb.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MemoryGamesController : MVCBaseController
    {
        private AuthContext db = new AuthContext();

        // GET: Admin/MemoryGames/Create
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

            MemoryGameCreateViewModel viewModel = new MemoryGameCreateViewModel();
            viewModel.Inicializar(id);
            
            viewModel.MemoryGame = new MemoryGame();
            viewModel.MemoryGame.TipoJuegoOnlineId = (int)TiposDeJuegosOnlineId.MemoryGame;
            viewModel.MemoryGame.JuegoOnlineId = id;
            viewModel.MemoryGame.SubTemaId = JuegoOnline.SubTemaId;
            return View(viewModel);
        }
        
        // POST: Admin/MemoryGames/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MemoryGameCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {

                viewModel.MemoryGame.Descripcion = viewModel.MemoryGame.PalabraImagen;

                db.MemoryGames.Add(viewModel.MemoryGame);
                db.SaveChanges();


                viewModel.MemoryGame.UrlImagen = viewModel.MemoryGame.Id + ".jpg";

                string nameAndLocation = "~/media/upload/MemoryGame/" + viewModel.MemoryGame.UrlImagen;
           
                       FileUploadHelper.SubirImagenCuadrada(viewModel.ImageFile, 256, 256, Server.MapPath(Url.Content(nameAndLocation)), ImageFormat.Png);

                db.Entry(viewModel.MemoryGame).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Create", "MemoryGames", new { id = viewModel.MemoryGame.JuegoOnlineId});
            }


            viewModel.Inicializar(viewModel.MemoryGame.JuegoOnlineId);
            return View(viewModel);
        }

        

        // GET: Admin/MemoryGames/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MemoryGame MemoryGame = db.MemoryGames.Find(id);
            if (MemoryGame == null)
            {
                return HttpNotFound();
            }
            if (MemoryGame.UrlImagen != null)
            {
                var fullPath = Request.MapPath("~/media/upload/MemoryGame/" + MemoryGame.UrlImagen);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }

            db.MemoryGames.Remove(MemoryGame);
            db.SaveChanges();
            return RedirectToAction("Create", "MemoryGames", new { id = MemoryGame.JuegoOnlineId });
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

