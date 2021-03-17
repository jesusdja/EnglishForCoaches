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
using System.Security.Claims;
using EnglishForCoachesWeb.DataAccess;

namespace EnglishForCoachesWeb.Areas.Alumno.Controllers
{
    [Authorize(Roles = "Alumno,Admin")]
    public class FillTheGapsController : MVCBaseController
    {
        private AuthContext db = new AuthContext();

        public ActionResult Index(int id, bool? mistakes)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var bloque = BloqueDataAccess.ObtenerBloques(db).Include(bl => bl.Area).Include(bl => bl.SubTema.Tema).FirstOrDefault(b => b.BloqueId == id);

            if (bloque == null)
                return HttpNotFound();

            if (!ComprobarAccesoSubTema(bloque.SubTemaId))
                return RedirectToAction("SinAcceso", "Home", new { Area = "Alumno" });
            if (bloque.AreaId==9)
                return RedirectToAction("SinAcceso", "Home", new { Area = "Alumno" });

            bool noMostrarMistakes = ContenidoHelper.MarcarEjercicioHecho(id);

            var userId = ((ClaimsIdentity)HttpContext.User.Identity).FindFirst("UserId").Value;
            List<FillTheGap> fillTheGaps = new List<FillTheGap>();
            if (mistakes.GetValueOrDefault())
            {
                var mistakeIds = db.Mistakes.Where(mist => mist.BloqueId == id && mist.AlumnoId == userId).Select(mist => mist.PreguntaId).ToList();
                fillTheGaps = db.FillTheGaps.Where(sk => sk.BloqueId == id && mistakeIds.Contains(sk.Id)).ToList();
            }
            else
            {
                fillTheGaps = db.FillTheGaps.Where(sk => sk.BloqueId == id).ToList();
            }
            fillTheGaps.Shuffle();

            FillTheGapIndexViewModel viewModel = new FillTheGapIndexViewModel();
            viewModel.bloque = bloque;
            viewModel.fillTheGaps = fillTheGaps;
            if (noMostrarMistakes)
            {
                viewModel.mistakes = new List<int>();
            }
            else
            {
                viewModel.mistakes = db.Mistakes.Where(mist => mist.BloqueId == id && mist.AlumnoId == userId).Select(mist => mist.PreguntaId).ToList();
            }

            return View(viewModel);
        }

        public JsonResult GetFillTheGap(int id)
        {
            FillTheGap fillTheGap = db.FillTheGaps.Find(id);
            Bloque bloque = db.Bloques.Find(fillTheGap.BloqueId);
            PreguntaFillTheGap pregunta = new PreguntaFillTheGap()
            {
                Enunciado = fillTheGap.Enunciado,
                Explicacion = bloque.Explicacion
            };

            return Json(pregunta, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Contestar(int id, string respuesta)
        {
            respuesta = respuesta.Replace(',', '#');
            FillTheGap fillTheGap = db.FillTheGaps.Find(id);
            ResultadoFillTheGap resultado = new ResultadoFillTheGap();
            if (fillTheGap.Respuestas.ToLower() == respuesta.ToLower())
            {
                resultado.Correcto = true;
                AuthRepository authRepository = new AuthRepository();
                ApplicationUser user = authRepository.FindByName(User.Identity.Name);

                user.PuntosActual = user.PuntosActual + 1;
                user.PuntosTotal = user.PuntosTotal + 1;

                var userResult = authRepository.Update(user);
                ContenidoHelper.QuitarMistake(fillTheGap.BloqueId, id);
            }
            else
            {
                resultado.Correcto = false;
                string[] _arraySoluciones= fillTheGap.Respuestas.ToLower().Split('#');
                string[] _arrayRespuestas=respuesta.ToLower().Split('#');

                resultado.correctas = new List<bool>();
                for (int i = 0; i < _arraySoluciones.Length; i++)
                    resultado.correctas.Add((_arraySoluciones[i] == _arrayRespuestas[i]));
            }
            if (string.IsNullOrEmpty(fillTheGap.Explicacion) ||fillTheGap.Explicacion == "-")
            {
                if (resultado.Correcto)
                {
                    resultado.Explicacion = "Correcto";
                }
                else
                {
                    resultado.Explicacion = "Incorrecto";
                }
            }
            else
            {
                resultado.Explicacion = fillTheGap.Explicacion;
            }

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
