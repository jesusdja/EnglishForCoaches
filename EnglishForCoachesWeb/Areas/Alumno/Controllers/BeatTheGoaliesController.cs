using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using EnglishForCoachesWeb.Database;
using EnglishForCoachesWeb.Database.JuegoOnline;
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
    public class BeatTheGoaliesController : MVCBaseController
    {
        private AuthContext db = new AuthContext();

        // GET: Admin/Contenidos
        public ActionResult Index(int id, bool? mistakes)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var juegoOnline = JuegoOnlineDataAccess.ObtenerJuegoOnlines(db).Include(bl => bl.SubTema.Tema).FirstOrDefault(b => b.JuegoOnlineId == id);

            if (juegoOnline == null)
                return HttpNotFound();

            if (!ComprobarAccesoSubTema(juegoOnline.SubTemaId))
                return RedirectToAction("SinAcceso", "Home", new { Area = "Alumno" });



            bool noMostrarMistakes = JuegoOnlineHelper.MarcarJuegoOnlineHecho(id);

            List<BeatTheGoalie> BeatTheGoalies = new List<BeatTheGoalie>();

            BeatTheGoalies = db.BeatTheGoalies.Where(sk => sk.JuegoOnlineId == id).ToList();

            BeatTheGoalies.Shuffle();

            BeatTheGoalieIndexViewModel viewModel = new BeatTheGoalieIndexViewModel();
            viewModel.juegoOnline = juegoOnline;
            viewModel.BeatTheGoalies = BeatTheGoalies;

            viewModel.mistakes = new List<int>();

            return View(viewModel);
        }

        public JsonResult GetBeatTheGoalie(int id)
        {
            BeatTheGoalie BeatTheGoalie = db.BeatTheGoalies.Find(id);
            PreguntaBeatTheGoalie pregunta = new PreguntaBeatTheGoalie()
            {
                Audio = BeatTheGoalie.FicheroAudio,
                Enunciado = BeatTheGoalie.Enunciado,
                Respuesta1 = BeatTheGoalie.Respuesta1,
                Respuesta2 = BeatTheGoalie.Respuesta2,
                Respuesta3 = BeatTheGoalie.Respuesta3,
                Respuesta4 = BeatTheGoalie.Respuesta4
            };

            return Json(pregunta, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Contestar(int id, int respuesta)
        {
            BeatTheGoalie BeatTheGoalie = db.BeatTheGoalies.Find(id);
            ResultadoBeatTheGoalie resultado = new ResultadoBeatTheGoalie();
            if (BeatTheGoalie.RespuestaCorrecta == respuesta)
            {
                resultado.Correcto = true;
                AuthRepository authRepository = new AuthRepository();
                ApplicationUser user = authRepository.FindByName(User.Identity.Name);

                user.PuntosActual = user.PuntosActual + 1;
                user.PuntosTotal = user.PuntosTotal + 1;

                var userResult = authRepository.Update(user);
            }
            else
            {
                resultado.Correcto = false;
            }
            if (BeatTheGoalie.Explicacion == "-")
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
                resultado.Explicacion = BeatTheGoalie.Explicacion;
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

