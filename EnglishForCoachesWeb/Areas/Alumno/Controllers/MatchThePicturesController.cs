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
using EnglishForCoachesWeb.Helpers;
using EnglishForCoachesWeb.Auth;
using EnglishForCoachesWeb.DataAccess;

namespace EnglishForCoachesWeb.Areas.Alumno.Controllers
{
    [Authorize(Roles = "Alumno,Admin")]
    public class MatchThePicturesController : MVCBaseController
    {
        private AuthContext db = new AuthContext();

        // GET: Admin/Contenidos
        public ActionResult Index(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var bloque = BloqueDataAccess.ObtenerBloques(db).Include(bl => bl.Area).Include(bl => bl.SubTema.Tema).FirstOrDefault(b => b.BloqueId == id);

            if (bloque == null)
            {
                return HttpNotFound();
            }
            if (!ComprobarAccesoSubTema(bloque.SubTemaId))
            {
                return RedirectToAction("SinAcceso", "Home", new { Area = "Alumno" });
            }

            ContenidoHelper.MarcarEjercicioHecho(id);

            var MatchThePictures = db.MatchThePictures.Where(sk => sk.BloqueId == id).ToList();
            MatchThePictures.Shuffle();

            MatchThePictureIndexViewModel viewModel = new MatchThePictureIndexViewModel();
            viewModel.bloque = bloque;
            viewModel.MatchThePictures = MatchThePictures;


            viewModel.Imagenes = MatchThePictures.Select(mtw=>mtw.UrlImagen).ToList();
            viewModel.Imagenes.Shuffle();
            viewModel.Palabras = MatchThePictures.Select(mtw => mtw.PalabraImagen).ToList();
            viewModel.Palabras.Shuffle();
            return View(viewModel);
        }
        public JsonResult Contestar(int id, string[] respuestas)
        {
            var MatchThePictures = db.MatchThePictures.Where(sk => sk.BloqueId == id).ToList();
            MatchThePictureResultado resultado = new MatchThePictureResultado();

            var respuestasCorrectas = MatchThePictures.Select(mtw => mtw.UrlImagen + mtw.PalabraImagen).ToList();
            bool[] correctas = new bool[respuestas.Length];
            int puntosGanados = 0;
            for(int i=0;i<respuestas.Length;i++)
            {
                string respuesta = respuestas[i];
                if (respuestasCorrectas.Contains(respuesta))
                {
                    puntosGanados++;
                    correctas[i] = true;
                }else
                {
                    correctas[i] = false;
                }
            }
            if (puntosGanados>0)
            {
                AuthRepository authRepository = new AuthRepository();
                ApplicationUser user = authRepository.FindByName(User.Identity.Name);

                user.PuntosActual = user.PuntosActual + puntosGanados;
                user.PuntosTotal = user.PuntosTotal + puntosGanados;

                var userResult = authRepository.Update(user);
            }

            resultado.Correctas = correctas;

            return Json(resultado, JsonRequestBehavior.AllowGet);
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
