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
using EnglishForCoachesWeb.Database.Varios;

namespace EnglishForCoachesWeb.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class VideosController : MVCBaseController
    {
        private AuthContext db = new AuthContext();


        // GET: Admin/Videos
        public ActionResult Index()
        {
            ActualizarThumbs();

            VideoIndexViewModel viewModel = new VideoIndexViewModel();
            var busqueda = db.Videos.OrderByDescending(not=>not.VideoId).ToList();
            viewModel.Pagina = 1;

            viewModel.CalcularPaginacion( busqueda.Count());
            viewModel.listadoVideos = busqueda.Take(viewModel.resultadosPorPagina).ToList();



            return View(viewModel);
        }

        private void ActualizarThumbs()
        {
            var videosSinThumb = db.Videos.Where(v => v.ThumbNail.Contains("default")|| v.ThumbNail.Contains(""));
           foreach(var vid in videosSinThumb)
            {
                VimeoHelper.GetThumbnail(vid.UrlVideo, vid.VideoId);
            }
        }



        // POST: Admin/Videos/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(VideoIndexViewModel viewModel)
        {
            ActualizarThumbs();
            var busqueda = db.Videos.OrderByDescending(not => not.VideoId).ToList();

            if (!string.IsNullOrWhiteSpace(viewModel.TextoBusqueda))
            {
                busqueda = busqueda.Where(x => x.Titulo.Contains(viewModel.TextoBusqueda)).ToList();
            }

            viewModel.CalcularPaginacion( busqueda.Count());

            int skip = (viewModel.Pagina - 1) * viewModel.resultadosPorPagina;
            viewModel.listadoVideos = busqueda.Skip(skip).Take(viewModel.resultadosPorPagina).ToList();
            return View(viewModel);
        }

        // GET: Admin/Videos/Create
        public ActionResult Create( )
        {            VideoCreateViewModel viewModel = new VideoCreateViewModel();
            
            return View(viewModel);
        }

        // POST: Admin/Videos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VideoCreateViewModel viewModel)
        {
            //fichero pdf
            //"~/media/upload/Videos_puntos/"

            if (ModelState.IsValid)
            {
                viewModel.Video.Fecha = DateTime.Now;
               
                db.Videos.Add(viewModel.Video);
                db.SaveChanges();
             

                    string nameAndLocation = "~/media/upload/VideosTemp/" + viewModel.Video.VideoId + "_" + viewModel.File.FileName.Replace(" ","");
                    viewModel.File.SaveAs(Server.MapPath(nameAndLocation));


                    string resultado = VimeoHelper.UploadVideo(nameAndLocation, viewModel.Video.VideoId);
                    string fullPath = Request.MapPath(nameAndLocation);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                if (resultado!= "OK")
                {
                    ModelState.AddModelError("File", resultado);

                    db.Videos.Remove(viewModel.Video);
                    db.SaveChanges();
                    return View(viewModel);
                }
                


                return RedirectToAction("Index",new { });
            }
            return View(viewModel);
        }

 
        // GET: Admin/Videos/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Video Video = db.Videos.Find(id);
            if (Video == null)
            {
                return HttpNotFound();
            }

            VimeoHelper.DeleteFiles(Video.UrlVideo);

            //Eliminar subtema
            db.Videos.Remove(Video);
            db.SaveChanges();
            return RedirectToAction("Index", new { });
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
