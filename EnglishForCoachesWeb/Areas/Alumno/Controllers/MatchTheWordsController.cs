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
    public class MatchTheWordsController : MVCBaseController
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

            var MatchTheWords = db.MatchTheWords.Where(sk => sk.BloqueId == id).ToList();
            MatchTheWords.Shuffle();

            MatchTheWordIndexViewModel viewModel = new MatchTheWordIndexViewModel();
            viewModel.bloque = bloque;
            viewModel.MatchTheWords = MatchTheWords;


            viewModel.Preguntas = MatchTheWords.Select(mtw=>mtw.Pregunta).ToList();
            viewModel.Preguntas.Shuffle();
            viewModel.Respuestas = MatchTheWords.Select(mtw => mtw.Respuesta).ToList();
            viewModel.Respuestas.Shuffle();

            return View(viewModel);
        }
        public JsonResult Contestar(int id, string[] respuestas)
        {
            var matchtheWords = db.MatchTheWords.Where(sk => sk.BloqueId == id).ToList();
            MatchTheWordResultado resultado = new MatchTheWordResultado();

            var respuestasCorrectas = matchtheWords.Select(mtw => mtw.Pregunta + mtw.Respuesta).ToList();
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
