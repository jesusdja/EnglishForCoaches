using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using EnglishForCoachesWeb.Database;
using EnglishForCoachesWeb.Database.Ejercicios;
using EnglishForCoachesWeb.Areas.Alumno.Models;
using EnglishForCoachesWeb.Controllers;
using EnglishForCoachesWeb.Helpers;
using EnglishForCoachesWeb.Auth;
using System.Security.Claims;
using System.Collections.Generic;
using EnglishForCoachesWeb.DataAccess;

namespace EnglishForCoachesWeb.Areas.Alumno.Controllers
{
    [Authorize(Roles = "Alumno,Admin")]
    public class TrueFalsesController : MVCBaseController
    {
        private AuthContext db = new AuthContext();

        // GET: Admin/Contenidos
        public ActionResult Index(int id,bool? mistakes)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var bloque = BloqueDataAccess.ObtenerBloques(db).Include(bl => bl.Area).Include(bl => bl.SubTema.Tema).FirstOrDefault(b => b.BloqueId == id);

            if (bloque == null)
                return HttpNotFound();

            if (!ComprobarAccesoSubTema(bloque.SubTemaId))
                return RedirectToAction("SinAcceso", "Home", new { Area = "Alumno" });

            if (bloque.AreaId == 9)
                return RedirectToAction("SinAcceso", "Home", new { Area = "Alumno" });

            bool noMostrarMistakes = ContenidoHelper.MarcarEjercicioHecho(id);

            var userId = ((ClaimsIdentity)HttpContext.User.Identity).FindFirst("UserId").Value;
            List<TrueFalse> TrueFalses = new List<TrueFalse>();
            if(mistakes.GetValueOrDefault())
            {
                var mistakeIds = db.Mistakes.Where(mist => mist.BloqueId == id && mist.AlumnoId == userId).Select(mist => mist.PreguntaId).ToList();
                TrueFalses = db.TrueFalses.Where(sk => sk.BloqueId == id && mistakeIds.Contains(sk.Id)).ToList();
            }
            else
            {
                TrueFalses = db.TrueFalses.Where(sk => sk.BloqueId == id).ToList();
            }

            TrueFalses.Shuffle();

            TrueFalseIndexViewModel viewModel = new TrueFalseIndexViewModel();
            viewModel.bloque = bloque;
            viewModel.TrueFalses = TrueFalses;
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

        public JsonResult GetTrueFalse(int id)
        {
            TrueFalse TrueFalse = db.TrueFalses.Find(id);
            Bloque bloque = db.Bloques.Find(TrueFalse.BloqueId);
            PreguntaTrueFalse pregunta = new PreguntaTrueFalse()
            {
                UrlImagen = TrueFalse.UrlImagen,
                Enunciado = TrueFalse.Enunciado,
                Audio = TrueFalse.Audio,
                Explicacion = bloque.Explicacion
            };

            return Json(pregunta, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Contestar(int id, bool respuesta)
        {
            TrueFalse TrueFalse = db.TrueFalses.Find(id);
            ResultadoTrueFalse resultado = new ResultadoTrueFalse();
            if (TrueFalse.RespuestaCorrecta == respuesta)
            {
                resultado.Correcto = true;
                AuthRepository authRepository = new AuthRepository();
                ApplicationUser user = authRepository.FindByName(User.Identity.Name);

                user.PuntosActual = user.PuntosActual + 1;
                user.PuntosTotal = user.PuntosTotal + 1;

                var userResult = authRepository.Update(user);

                ContenidoHelper.QuitarMistake(TrueFalse.BloqueId, id);
            }
            else
            {
                resultado.Correcto = false;
            }
            if (TrueFalse.Explicacion == "-")
            {
                if (resultado.Correcto)
                {
                    resultado.Explicacion = "Correcto";
                }else
                {
                    resultado.Explicacion = "Incorrecto";                    
                }
            }
            else
            {
                resultado.Explicacion = TrueFalse.Explicacion;
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

