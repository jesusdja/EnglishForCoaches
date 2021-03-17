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
    public class FillTheBoxsController : MVCBaseController
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
            List<FillTheBox> fillTheBoxs = new List<FillTheBox>();
            if (mistakes.GetValueOrDefault())
            {
                var mistakeIds = db.Mistakes.Where(mist => mist.BloqueId == id && mist.AlumnoId == userId).Select(mist => mist.PreguntaId).ToList();
                fillTheBoxs = db.FillTheBoxs.Where(sk => sk.BloqueId == id && mistakeIds.Contains(sk.Id)).ToList();
            }
            else
            {
                fillTheBoxs = db.FillTheBoxs.Where(sk => sk.BloqueId == id).ToList();
            }
            fillTheBoxs.Shuffle();

            FillTheBoxIndexViewModel viewModel = new FillTheBoxIndexViewModel();
            viewModel.bloque = bloque;
            viewModel.fillTheBoxs = fillTheBoxs;
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

        public JsonResult GetFillTheBox(int id)
        {
            FillTheBox fillTheBox = db.FillTheBoxs.Find(id);
            var enunciado = fillTheBox.Enunciado.Replace("#","|#|").Split('|').ToList();
      
            var respuestas = fillTheBox.Respuestas.Split('#').ToList();
            if (!string.IsNullOrWhiteSpace(fillTheBox.RespuestasIncorrectas))
            {
                respuestas.AddRange(fillTheBox.RespuestasIncorrectas.Split('#').ToList());
            }
            respuestas.Shuffle();
            PreguntaFillTheBox pregunta = new PreguntaFillTheBox()
            {
                Titulo=fillTheBox.Titulo,
                Audio= fillTheBox.FicheroAudio,
                Imagen=fillTheBox.UrlImagen,
                Enunciado = enunciado.ToArray(),
                Respuestas= respuestas.ToArray()
            };

        
         
            return Json(pregunta, JsonRequestBehavior.AllowGet);
        }


        public string ReplaceFirst(string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }

        public JsonResult Contestar(int id, string[] respuestas)
        {
                FillTheBox fillTheBox = db.FillTheBoxs.Find(id);
            bool[] correctas = new bool[respuestas.Length];
            bool correcto = true;

            string enunciadoOriginal = fillTheBox.Enunciado;
            enunciadoOriginal = enunciadoOriginal.Replace("#", "|#|");

            var enunciado = enunciadoOriginal.Split('|').ToArray();


            var respuestaReal = fillTheBox.Respuestas.Split('#').ToArray();



            int contadorTotal = 0;
            int contadorrespuesta = 0;
            for (int i = 0; i < enunciado.Length; i++)
            {
                if (enunciado[i] != "")
                {
                    if (enunciado[i] == respuestas[contadorTotal])
                    {
                        correctas[contadorTotal] = true;
                    }
                    else if (enunciado[i] == "#")
                    {
                        if (contadorrespuesta < respuestas.Length)
                        {
                            if (respuestaReal[contadorrespuesta] == respuestas[contadorTotal])
                            {
                                correctas[contadorTotal] = true;
                            }
                            else
                            {
                                correcto = false;
                                correctas[contadorTotal] = false;
                            }
                        }
                        contadorrespuesta++;
                    }
                    else
                    {
                        correcto = false;
                        correctas[contadorTotal] = false;
                    }
                    contadorTotal++;
                }
            }

            ResultadoFillTheBox resultado = new ResultadoFillTheBox();

            resultado.correctas = correctas;

            resultado.Correcto = correcto;
            if (correcto)
            {
                AuthRepository authRepository = new AuthRepository();
                ApplicationUser user = authRepository.FindByName(User.Identity.Name);

                user.PuntosActual = user.PuntosActual + 1;
                user.PuntosTotal = user.PuntosTotal + 1;

                var userResult = authRepository.Update(user);
            }
            resultado.Explicacion = fillTheBox.Explicacion;
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
