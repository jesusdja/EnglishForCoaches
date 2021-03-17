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
using System.Security.Claims;
using EnglishForCoachesWeb.DataAccess;

namespace EnglishForCoachesWeb.Areas.Alumno.Controllers
{
    [Authorize(Roles = "Alumno,Admin")]
    public class VocabulariosController : MVCBaseController
    {
        private AuthContext db = new AuthContext();

        // GET: Alumno/Vocabularios
        public ActionResult Index(string idioma,string letra,string textoBusqueda)
        {
            if(string.IsNullOrEmpty(idioma))
            {
                idioma = "en";
            }
            if (string.IsNullOrEmpty(letra))
            {
                letra = "A";
            }
            if (string.IsNullOrEmpty(textoBusqueda))
            {
                textoBusqueda = "";
            }else
            {

                letra = "";
            }


            VocabulariosIndexViewModel viewModel = new VocabulariosIndexViewModel();
            viewModel.Idioma = idioma;
            viewModel.Letra = letra;
            viewModel.TextoBusqueda = textoBusqueda;
            var userId = ((ClaimsIdentity)User.Identity).FindFirst("UserId").Value;
            viewModel.glosario = db.VocabularioGlosarios.Where(vg => vg.AlumnoId == userId).Select(vg => vg.VocabularioId).ToList();

            var vocabulario = ObtenerVocabulario(textoBusqueda, letra, idioma);
            viewModel.vocabulario = vocabulario;

            return View(viewModel);
        }

        private List<Vocabulario> ObtenerVocabulario(string textoBusqueda, string letra, string idioma)
        {
            var vocabularios = db.Vocabularios;
            var vocabulario = new List<Vocabulario>();
            if (idioma == "en")
            {
                vocabulario = vocabularios.Include(v=>v.CategoriaVocabulario).Include(v => v.DefinicionVocabularios).Include(v => v.Frases)
                    .OrderBy(v => v.Palabra_en).ToList();

                if (!string.IsNullOrEmpty(textoBusqueda))
                {
                    vocabulario = vocabulario.Where(v => v.Palabra_en.ToUpper().Contains(textoBusqueda.ToUpper())).ToList();
                }
                else
                {
                    vocabulario = vocabulario.Where(v => v.Palabra_en.ToUpper().StartsWith(letra)).ToList();
                }
            }
            else
            {
                vocabulario = vocabularios.Include(v => v.CategoriaVocabulario).Include(v => v.DefinicionVocabularios).Include(v => v.Frases).OrderBy(v => v.Palabra_es).ToList();
                if (!string.IsNullOrEmpty(textoBusqueda))
                {
                    vocabulario = vocabulario.Where(v => v.Palabra_es.ToUpper().Contains(textoBusqueda.ToUpper())).ToList();
                }else
                {
                    vocabulario = vocabulario.Where(v => v.Palabra_es.ToUpper().StartsWith(letra)).ToList();
                }
            }
                return vocabulario;
        }

        // POST: Alumno/Vocabularios
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(VocabulariosIndexViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(viewModel.Idioma))
                {
                    viewModel.Idioma = "en";
                }
                if (string.IsNullOrEmpty(viewModel.TextoBusqueda))
                {
                    viewModel.TextoBusqueda = "";
                    if (string.IsNullOrEmpty(viewModel.Letra))
                    {
                        viewModel.Letra = "A";
                    }
                }
                else
                {
                    viewModel.Letra = "";
                }
                var vocabulario = ObtenerVocabulario(viewModel.TextoBusqueda, viewModel.Letra, viewModel.Idioma);

                viewModel.vocabulario = vocabulario;
            }else
            {
                viewModel.vocabulario = new List<Vocabulario>();
            }
            var userId = ((ClaimsIdentity)User.Identity).FindFirst("UserId").Value;
            viewModel.glosario = db.VocabularioGlosarios.Where(vg => vg.AlumnoId == userId).Select(vg => vg.VocabularioId).ToList();


            return View(viewModel);
        }


        // GET: Alumno/Vocabularios/Gramatica
        public ActionResult Gramatica(int id, int gramaticaId, string idioma)
        {
            if (id == null|| gramaticaId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var subtema = SubTemaDataAccess.ObtenerSubTemas(db).Include(s => s.Tema).SingleOrDefault(s => s.SubTemaId == id);
            var gramatica = GramaticaDataAccess.ObtenerGramaticas(db).Include(gt=>gt.Vocabularios.Select(v=>v.CategoriaVocabulario)).FirstOrDefault(g=>g.GramaticaId==gramaticaId);
            if (subtema == null|| gramatica==null)
            {
                return HttpNotFound();
            }

            if (!ComprobarAccesoSubTema(id))
            {
                return RedirectToAction("SinAcceso", "Home", new { Area = "Alumno" });
            }

            if (string.IsNullOrEmpty(idioma))
            {
                idioma = "en";
            }
            
          
            var vocabulario = new List<Vocabulario>();
            if (idioma == "en")
            {
                 vocabulario = gramatica.Vocabularios.OrderBy(v => v.Palabra_en).ToList();
            }else
            {
                 vocabulario = gramatica.Vocabularios.OrderBy(v => v.Palabra_es).ToList();
            }

            VocabulariosGramaticaViewModel viewModel = new VocabulariosGramaticaViewModel();
            viewModel.vocabulario = vocabulario;
            viewModel.Subtema = subtema;
            viewModel.Gramatica = gramatica;
            viewModel.Idioma = idioma;

            var userId = ((ClaimsIdentity)User.Identity).FindFirst("UserId").Value;
            viewModel.glosario = db.VocabularioGlosarios.Where(vg => vg.AlumnoId == userId).Select(vg => vg.VocabularioId).ToList();

            return View(viewModel);
        }



        // GET: Alumno/Vocabularios/Glosario
        public ActionResult Glosario(string idioma, string letra, string textoBusqueda)
        {
            if (string.IsNullOrEmpty(idioma))
            {
                idioma = "en";
            }
            if (string.IsNullOrEmpty(letra))
            {
                letra = "A";
            }
            if (string.IsNullOrEmpty(textoBusqueda))
            {
                textoBusqueda = "";
            }
            else
            {

                letra = "";
            }


            VocabulariosIndexViewModel viewModel = new VocabulariosIndexViewModel();
            viewModel.Idioma = idioma;
            viewModel.Letra = letra;
            viewModel.TextoBusqueda = textoBusqueda;
            var userId = ((ClaimsIdentity)User.Identity).FindFirst("UserId").Value;

            viewModel.glosario = db.VocabularioGlosarios.Where(vg => vg.AlumnoId == userId).Select(vg => vg.VocabularioId).ToList();

            var vocabulario = ObtenerVocabulario(textoBusqueda, letra, idioma);
            viewModel.vocabulario = vocabulario.Where(v=> viewModel.glosario.Contains(v.VocabularioId)).ToList();

            return View(viewModel);
        }

        // POST: Alumno/Vocabularios/Glosario
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Glosario(VocabulariosIndexViewModel viewModel)
        {
            var userId = ((ClaimsIdentity)User.Identity).FindFirst("UserId").Value;
            viewModel.glosario = db.VocabularioGlosarios.Where(vg => vg.AlumnoId == userId).Select(vg => vg.VocabularioId).ToList();
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(viewModel.Idioma))
                {
                    viewModel.Idioma = "en";
                }
                if (string.IsNullOrEmpty(viewModel.TextoBusqueda))
                {
                    viewModel.TextoBusqueda = "";
                    if (string.IsNullOrEmpty(viewModel.Letra))
                    {
                        viewModel.Letra = "A";
                    }
                }
                else
                {
                    viewModel.Letra = "";
                }
                var vocabulario = ObtenerVocabulario(viewModel.TextoBusqueda, viewModel.Letra, viewModel.Idioma);

                viewModel.vocabulario = vocabulario.Where(v => viewModel.glosario.Contains(v.VocabularioId)).ToList();
            }
            else
            {
                viewModel.vocabulario = new List<Vocabulario>();
            }
            return View(viewModel);
        }


        public JsonResult AddGlosario(int id)
        {
            var userId = ((ClaimsIdentity)User.Identity).FindFirst("UserId").Value;
            string resultado = "";
            VocabularioGlosario existe = db.VocabularioGlosarios.FirstOrDefault(vg => vg.VocabularioId == id && vg.AlumnoId == userId);
            if (existe != null)
            {
                db.VocabularioGlosarios.Remove(existe);
                db.SaveChanges();
                resultado = "Eliminado";
            }
            else
            {
                VocabularioGlosario vocabularioGlosario = new VocabularioGlosario()
                {
                    AlumnoId = userId,
                    Fecha = DateTime.Now,
                    VocabularioId = id
                };
                db.VocabularioGlosarios.Add(vocabularioGlosario);
                db.SaveChanges();
                resultado = "Añadido";
            }

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Frases(int id)
        {
            var frases = db.Vocabularios.Include(vo=>vo.Frases).First(fr => fr.VocabularioId == id).Frases;
            string resultado = "<ul>";
            foreach(var frase in frases)
            {
                resultado += "<li ><span style='font - weight: bold; color: blue;'>" + frase.Palabra_en + "</span> - " + frase.Palabra_es + "</li>";
            }
            resultado += "</ul>";
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
    }
}
