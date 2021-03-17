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
using EnglishForCoachesWeb.Database.JuegoOnline;
using System.Security.Claims;

namespace EnglishForCoachesWeb.Areas.Alumno.Controllers
{
    [Authorize(Roles = "Alumno,Admin")]
    public class AhorcadoController : MVCBaseController
    {
        private AuthContext db = new AuthContext();

        // GET: Admin/Contenidos
        public ActionResult Index(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var juegoOnline = JuegoOnlineDataAccess.ObtenerJuegoOnlines(db).Include(bl => bl.SubTema.Tema).FirstOrDefault(b => b.JuegoOnlineId == id);

            if (juegoOnline == null)
            {
                return HttpNotFound();
            }
            if (!ComprobarAccesoSubTema(juegoOnline.SubTemaId))
            {
                return RedirectToAction("SinAcceso", "Home", new { Area = "Alumno" });
            }

            bool noMostrarMistakes = JuegoOnlineHelper.MarcarJuegoOnlineHecho(id);

            List<Ahorcado> ahorcados = new List<Ahorcado>();
            ahorcados = db.Ahorcado.Where(sk => sk.JuegoOnlineId == id).ToList();


            ahorcados.Shuffle();
            AhorcadoIndexViewModel viewModel = new AhorcadoIndexViewModel();
            viewModel.juegoOnline = juegoOnline;
            viewModel.Ahorcados = ahorcados;


            return View(viewModel);
        }

        public JsonResult GetAhorcado(int id)
        {
            Ahorcado Ahorcado = db.Ahorcado.Find(id);
            PreguntaAhorcado pregunta = new PreguntaAhorcado()
            {
                Audio = Ahorcado.Audio,
                Imagen = Ahorcado.UrlImagen,
                Enunciado = Ahorcado.Descripcion,
                MascaraInicial = GenerarMascara(Ahorcado.Respuesta, new List<string>())
            };

            return Json(pregunta, JsonRequestBehavior.AllowGet);
        }


        private int[] GetCharacterPositions(string palabra, string letra)
        {
            var foundIndexes = new List<int>();

            for (int i = palabra.IndexOf(letra); i > -1; i = palabra.IndexOf(letra, i + 1))
            {
                foundIndexes.Add(i);
            }
            return foundIndexes.ToArray();
        }


        private string GenerarMascara(string palabra,List<string> letrasAcertadas) {
            var mascara = "";

            for (var i = 0; i < palabra.Length; i++) {
                if (palabra[i] == '-' || palabra[i] == ' ' || palabra[i] == '(' || palabra[i] == ')'
                    || palabra[i] == '\'' || palabra[i] == '.') {
                    mascara += palabra[i];

                } else {
                    if (letrasAcertadas.Contains(palabra[i].ToString())) {
                        mascara += palabra[i];
                    } else {
                        mascara += "_";
                    }
                }
            }
            return mascara;
        }

        public JsonResult Contestar(int id, RespuestaAhorcado respuesta)
        {

            var ahorcado = db.Ahorcado.FirstOrDefault(sk => sk.Id == id);
            AhorcadoResultado resultado = new AhorcadoResultado();
            if(respuesta.letrasAcertadas==null)
            {
                respuesta.letrasAcertadas = new string[0];
            }


            var letrasAcertadas = respuesta.letrasAcertadas.ToList();

            bool correcto = false;
            if (ahorcado.Respuesta.ToUpper().Contains( respuesta.letra.ToUpper()))
            {
                correcto = true;
                resultado.posiciones = GetCharacterPositions(ahorcado.Respuesta.ToUpper(),respuesta.letra.ToUpper());

                foreach(var pos in resultado.posiciones)
                {
                    respuesta.palabra= respuesta.palabra.Remove(pos, 1).Insert(pos, respuesta.letra.ToUpper());
                }

                letrasAcertadas.Add(respuesta.letra.ToUpper());
            }
        
            
            resultado.Correcto = correcto;

            // comprobar si está finalizado
            resultado.Finalizado = false;
            var palabra = respuesta.palabra;
            if (ahorcado.Respuesta.ToUpper() == palabra.ToUpper())
            {
                resultado.Finalizado = true;
            }

            resultado.Explicacion = ahorcado.Explicacion;
            resultado.Mascara = GenerarMascara(ahorcado.Respuesta.ToUpper(), letrasAcertadas);

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        private string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
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

    public class RespuestaAhorcado
    {
        public string[] letrasAcertadas { get; set; }
        public string letra { get; set; }
        public string palabra { get; set; }
    }
}
