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
    public class SopaLetrasController : MVCBaseController
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

            var sopaLetras = db.SopaLetras.Include(cr=> cr.VocabularioSopaLetras).FirstOrDefault(sk => sk.JuegoOnlineId == id);

            SopaLetrasIndexViewModel viewModel = new SopaLetrasIndexViewModel();
            viewModel.juegoOnline = juegoOnline;
            viewModel.SopaLetras = sopaLetras;
            viewModel.Letras = new string[12][];
            viewModel.LetrasRespuesta = new string[12][];
   
            viewModel = ObtenerPalabrasydefiniciones(viewModel);
       
            return View(viewModel);
        }


        public ActionResult Completado(int id)
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
            SopaLetrasCompletadoViewModel viewModel = new SopaLetrasCompletadoViewModel();
            viewModel.juegoOnline = juegoOnline;

            var sopaLetras = db.SopaLetras.Include(cr => cr.VocabularioSopaLetras).FirstOrDefault(sk => sk.JuegoOnlineId == id);
            var listaVocabularioIds = sopaLetras.VocabularioSopaLetras.Select(vsl=>vsl.VocabularioId);
            viewModel.vocabulario = db.Vocabularios.Include(v => v.CategoriaVocabulario)
                .Include(v => v.DefinicionVocabularios).Include(v => v.Frases).Where(voca=> listaVocabularioIds.Contains(voca.VocabularioId))
                .OrderBy(v => v.Palabra_en).ToList();
            viewModel.Idioma = "en";
            var userId = ((ClaimsIdentity)User.Identity).FindFirst("UserId").Value;
            viewModel.glosario = db.VocabularioGlosarios.Where(vg => vg.AlumnoId == userId).Select(vg => vg.VocabularioId).ToList();


            return View(viewModel);
        }

            private List<string>  ObtenerPalabras(SopaLetras SopaLetras)
        {
            List<string> Vocabularios = new List<string>();
            foreach (var vocabularioSopa in SopaLetras.VocabularioSopaLetras)
            {
                var vocabulario = db.Vocabularios.Find(vocabularioSopa.VocabularioId);
                Vocabularios.Add(vocabulario.Palabra_en.ToLower());
            }
            return Vocabularios;
        }

        private SopaLetrasIndexViewModel ObtenerPalabrasydefiniciones(SopaLetrasIndexViewModel SopaLetrasVm)
        {
            List<string> Vocabularios = new List<string>();
            List<DefinicionSopaLetras> Definiciones = new List<DefinicionSopaLetras>();
            foreach (var vocabularioSopa in SopaLetrasVm.SopaLetras.VocabularioSopaLetras)
            {
                var vocabulario = db.Vocabularios.Find(vocabularioSopa.VocabularioId);
                Vocabularios.Add(vocabulario.Palabra_en.ToLower());
                Definiciones.Add(new DefinicionSopaLetras()
                {
                    Definicion = vocabulario.Definicion,
                    VocabularioId = vocabulario.VocabularioId
                });
            }
            SopaLetrasVm.Vocabularios = Vocabularios;
            SopaLetrasVm.Definiciones = Definiciones;



            return SopaLetrasVm;
        }

        public JsonResult Contestar(int id, RespuestaSopaLetras respuestas)
        {

            var sopaLetras = db.SopaLetras.Include(c=>c.CasillaSopaLetras).Include(c => c.VocabularioSopaLetras).FirstOrDefault(sk => sk.JuegoOnlineId == id);
            SopaLetrasResultado resultado = new SopaLetrasResultado();


            List<string> Vocabularios = ObtenerPalabras(sopaLetras);
            


            bool correcto = false;
            var palabra = respuestas.palabra;
            if (Vocabularios.Contains(respuestas.palabra))
            {
                correcto = true;
            }
            if (Vocabularios.Contains(Reverse(respuestas.palabra)))
            {
                 palabra = Reverse(respuestas.palabra);
                correcto = true;
            }
            
            resultado.Correcto = correcto;


            // comprobar si está finalizado
            resultado.Finalizado = false;
            var casillasTotales = sopaLetras.CasillaSopaLetras.Count(csl => !string.IsNullOrEmpty( csl.letra));
            int contestadas = respuestas.acertadas;
            if (correcto)
            {
                contestadas = contestadas + 1;

                var comentario = sopaLetras.VocabularioSopaLetras.FirstOrDefault(csl => csl.Vocabulario.Palabra_en.ToLower() == palabra);
 
                if (comentario != null)
                {
                    resultado.VocabularioId = comentario.VocabularioId;
                    resultado.FicheroAudio = comentario.Vocabulario.FicheroAudio;

                    
                    resultado.Comentario = comentario.Vocabulario.Palabra_en +"-"+ comentario.Vocabulario.Palabra_es + ": " + comentario.Comentario;
                }
            }
            if (Vocabularios.Count <= contestadas)
            {
                resultado.Finalizado = true;

                resultado.textoFinalizado = "";
                foreach (var comentario in sopaLetras.VocabularioSopaLetras)
                {
                    resultado.textoFinalizado += comentario.Vocabulario.Palabra_en + " - " + comentario.Vocabulario.Palabra_es + "\r\n";
                }
            }


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

    public class RespuestaSopaLetras
    {
        public int firstDiv_x { get; set; }
        public int firstDiv_y { get; set; }
        public int lastDiv_x { get; set; }
        public int lastDiv_y { get; set; }
        public int acertadas { get; set; }
        public string palabra { get; set; }
    }
}
