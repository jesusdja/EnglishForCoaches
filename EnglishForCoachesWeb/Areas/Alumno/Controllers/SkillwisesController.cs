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
    public class SkillwisesController : MVCBaseController
    {
        private AuthContext db = new AuthContext();

        // GET: Admin/Contenidos
        public ActionResult Index(int id,bool? mistakes)
        {
            if (id == null )
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var bloque = BloqueDataAccess.ObtenerBloques(db).Include(bl=> bl.Area).Include(bl => bl.SubTema.Tema).FirstOrDefault(b=>b.BloqueId==id);

            if (bloque == null)
            {
                return HttpNotFound();
            }
            if (!ComprobarAccesoSubTema(bloque.SubTemaId))
                return RedirectToAction("SinAcceso", "Home", new { Area = "Alumno" });

            bool noMostrarMistakes = ContenidoHelper.MarcarEjercicioHecho(id);



            var userId = ((ClaimsIdentity)HttpContext.User.Identity).FindFirst("UserId").Value;
            List<Skillwise> skillwises = new List<Skillwise>();
            if (mistakes.GetValueOrDefault())
            {
                var mistakeIds = db.Mistakes.Where(mist => mist.BloqueId == id && mist.AlumnoId == userId).Select(mist => mist.PreguntaId).ToList();
                skillwises = db.Skillwises.Where(sk => sk.BloqueId == id && mistakeIds.Contains(sk.Id)).ToList();
            }
            else
            {
                skillwises = db.Skillwises.Where(sk => sk.BloqueId == id).ToList();
            }


            skillwises.Shuffle();


            SkillwiseIndexViewModel viewModel = new SkillwiseIndexViewModel();
            viewModel.bloque = bloque;
            viewModel.skillwises = skillwises;
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


        public JsonResult GetSkillwise(int id)
        {
            Skillwise skillwise = db.Skillwises.Find(id);
            PreguntaSkillwise pregunta = new PreguntaSkillwise()
            {
                Enunciado = skillwise.Enunciado,
                Respuesta1 = skillwise.Respuesta1,
                Respuesta2 = skillwise.Respuesta2,
                Respuesta3 = skillwise.Respuesta3,
                Respuesta4 = skillwise.Respuesta4
            };

            return Json(pregunta, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Contestar(int id, int respuesta)
        {
            Skillwise skillwise = db.Skillwises.Find(id);
            ResultadoSkillwise resultado = new ResultadoSkillwise();
            if(skillwise.RespuestaCorrecta==respuesta)
            {
                resultado.Correcto = true;
                AuthRepository authRepository = new AuthRepository();
                ApplicationUser user = authRepository.FindByName(User.Identity.Name);

                user.PuntosActual = user.PuntosActual+1;
                user.PuntosTotal = user.PuntosTotal + 1;

                var userResult = authRepository.Update(user);
                ContenidoHelper.QuitarMistake(skillwise.BloqueId, id);
            }
            else
            {
                resultado.Correcto = false;
            }
            if(respuesta==1)
            {
                resultado.Explicacion = skillwise.Explicacion1;
            }
            if (respuesta == 2)
            {
                resultado.Explicacion = skillwise.Explicacion2;
            }
            if (respuesta == 3)
            {
                resultado.Explicacion = skillwise.Explicacion3;
            }
            if (respuesta == 4)
            {
                resultado.Explicacion = skillwise.Explicacion4;
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
