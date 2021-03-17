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
    public class WordByWordsController : MVCBaseController
    {
        private AuthContext db = new AuthContext();


        // GET: Admin/Contenidos
        public ActionResult Index(int id, bool? mistakes)
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

            bool noMostrarMistakes = ContenidoHelper.MarcarEjercicioHecho(id);

            var userId = ((ClaimsIdentity)HttpContext.User.Identity).FindFirst("UserId").Value;
            List<WordByWord> WordByWords = new List<WordByWord>();
            if (mistakes.GetValueOrDefault())
            {
                var mistakeIds = db.Mistakes.Where(mist => mist.BloqueId == id && mist.AlumnoId == userId).Select(mist => mist.PreguntaId).ToList();
                WordByWords = db.WordByWords.Where(sk => sk.BloqueId == id && mistakeIds.Contains(sk.Id)).ToList();
            }
            else
            {
                WordByWords = db.WordByWords.Where(sk => sk.BloqueId == id).ToList();
            }
            WordByWords.Shuffle();


            WordByWordIndexViewModel viewModel = new WordByWordIndexViewModel();
            viewModel.bloque = bloque;
            viewModel.WordByWords = WordByWords;
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
        public JsonResult GetWordByWord(int id)
        {
            WordByWord WordByWord = db.WordByWords.Find(id);
            var enunciado = WordByWord.Enunciado.Split('#').ToList();
            enunciado.Shuffle();
            PreguntaWordByWord pregunta = new PreguntaWordByWord()
            {
                Enunciado = enunciado.ToArray()
            };

            return Json(pregunta, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Contestar(int id, string[] respuestas)
        {
            WordByWord WordByWord = db.WordByWords.Find(id);
            bool[] correctas = new bool[respuestas.Length];
            bool correcto = true;
            var enunciado= WordByWord.Enunciado.Split('#').ToArray();
            for (int i = 0; i < enunciado.Length; i++)
            {
                if (enunciado[i] == respuestas[i])
                {
                    correctas[i] = true;
                }
                else
                {
                    correcto = false;
                    correctas[i] = false;
                }
            }

            WordByWordResultado resultado = new WordByWordResultado();

            resultado.Correctas = correctas;

            if (correcto)
            {
                AuthRepository authRepository = new AuthRepository();
                ApplicationUser user = authRepository.FindByName(User.Identity.Name);

                user.PuntosActual = user.PuntosActual + 1;
                user.PuntosTotal = user.PuntosTotal + 1;

                var userResult = authRepository.Update(user);
                ContenidoHelper.QuitarMistake(WordByWord.BloqueId, id);
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
