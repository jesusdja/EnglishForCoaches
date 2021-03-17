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
    public class TestsController : MVCBaseController
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
            List<Test> tests = new List<Test>();
            if(mistakes.GetValueOrDefault())
            {
                var mistakeIds = db.Mistakes.Where(mist => mist.BloqueId == id && mist.AlumnoId == userId).Select(mist => mist.PreguntaId).ToList();
                tests = db.Tests.Where(sk => sk.BloqueId == id && mistakeIds.Contains(sk.Id)).ToList();
            }
            else
            {
                tests = db.Tests.Where(sk => sk.BloqueId == id).ToList();
            }

            tests.Shuffle();

            TestIndexViewModel viewModel = new TestIndexViewModel();
            viewModel.bloque = bloque;
            viewModel.tests = tests;
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

        public JsonResult GetTest(int id)
        {
            Test test = db.Tests.Find(id);
            Bloque bloque = db.Bloques.Find(test.BloqueId);
            PreguntaTest pregunta = new PreguntaTest()
            {
                UrlImagen = test.UrlImagen,
                Enunciado = test.Enunciado,
                Respuesta1 = test.Respuesta1,
                Respuesta2 = test.Respuesta2,
                Respuesta3 = test.Respuesta3,
                Respuesta4 = test.Respuesta4,
                Explicacion = bloque.Explicacion
            };

            return Json(pregunta, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Contestar(int id, int respuesta)
        {
            Test test = db.Tests.Find(id);
            ResultadoTest resultado = new ResultadoTest();
            if (test.RespuestaCorrecta == respuesta)
            {
                resultado.Correcto = true;
                AuthRepository authRepository = new AuthRepository();
                ApplicationUser user = authRepository.FindByName(User.Identity.Name);

                user.PuntosActual = user.PuntosActual + 1;
                user.PuntosTotal = user.PuntosTotal + 1;

                var userResult = authRepository.Update(user);

                ContenidoHelper.QuitarMistake(test.BloqueId, id);
            }
            else
            {
                resultado.Correcto = false;
            }
            if (test.Explicacion == "-")
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
                resultado.Explicacion = test.Explicacion;
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

