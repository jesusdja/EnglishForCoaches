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
using EnglishForCoachesWeb.Areas.Alumno.Models;
using EnglishForCoachesWeb.Controllers;
using EnglishForCoachesWeb.Auth;
using System.Security.Claims;
using EnglishForCoachesWeb.DataAccess;

namespace EnglishForCoachesWeb.Areas.Alumno.Controllers
{
    [Authorize(Roles = "Alumno,Admin")]
    public class SubTemaVideosController : MVCBaseController
    {
        private AuthContext db = new AuthContext();


        // GET: Alumno/SubTemaVideos
        public ActionResult Index(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var subtema = SubTemaDataAccess.ObtenerSubTemas(db).Include(ex => ex.Tema).FirstOrDefault(b => b.SubTemaId == id);

            if (subtema == null)
            {
                return HttpNotFound();
            }
            var SubTemaVideos = SubTemaVideoDataAccess.ObtenerSubTemaVideos(db).Where(b => b.SubTemaId == id).ToList();
            SubTemaVideosIndexViewModel viewModel = new SubTemaVideosIndexViewModel();
            viewModel.SubTema = subtema;
            viewModel.SubTemaVideos = SubTemaVideos;
            return View(viewModel);
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
