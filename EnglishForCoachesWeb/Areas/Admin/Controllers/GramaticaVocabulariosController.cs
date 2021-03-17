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
using EnglishForCoachesWeb.Areas.Admin.Models;
using EnglishForCoachesWeb.Controllers;

namespace EnglishForCoachesWeb.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class GramaticaVocabulariosController : MVCBaseController
    {
        private AuthContext db = new AuthContext();


        // GET: Admin/GramaticaVocabularios
        public ActionResult Index(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var gramatica = db.Gramaticas.Include(s => s.SubTema.Tema).Include(g=>g.Vocabularios).SingleOrDefault(s => s.GramaticaId == id);
            if (gramatica == null)
            {
                return HttpNotFound();
            }

            GramaticaVocabularioIndexViewModel viewModel = new GramaticaVocabularioIndexViewModel();
            viewModel.listadoGramaticaVocabularios = gramatica.Vocabularios.OrderBy(au => au.Palabra_es).ToList();
            viewModel.Gramatica = gramatica;
            
            return View(viewModel);
        }
        


        // GET: Admin/Vocabularios
        public ActionResult Create(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var gramatica = db.Gramaticas.Include(s => s.SubTema.Tema).Include(g => g.Vocabularios).SingleOrDefault(s => s.GramaticaId == id);
            if (gramatica == null)
            {
                return HttpNotFound();
            }
            GramaticaVocabularioCreateViewModel viewModel = new GramaticaVocabularioCreateViewModel();
            viewModel.Gramatica = gramatica;
            var vocabularios = db.Vocabularios.OrderBy(au => au.Palabra_es).ToList();
            viewModel.listadoVocabularios = vocabularios.Take(viewModel.resultadosPorPagina).ToList();
            viewModel.Pagina = 1;

            viewModel.listadoVocabularioIncluidos = gramatica.Vocabularios.Select(v => v.VocabularioId).ToList();
            viewModel.CalcularPaginacion(vocabularios.Count());
            return View(viewModel);
        }

     

        // POST: Admin/AccesoUsuarios/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int id,GramaticaVocabularioCreateViewModel viewModel)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var gramatica = db.Gramaticas.Include(s => s.SubTema.Tema).Include(g => g.Vocabularios).SingleOrDefault(s => s.GramaticaId == id);
            if (gramatica == null)
            {
                return HttpNotFound();
            }
            var busqueda = db.Vocabularios.OrderBy(au => au.Palabra_es).ToList();

            if (!string.IsNullOrWhiteSpace(viewModel.TextoBusqueda))
            {
                busqueda = busqueda.Where(x => x.Palabra_es.Contains(viewModel.TextoBusqueda) || x.Palabra_en.Contains(viewModel.TextoBusqueda)).ToList();
            }

            viewModel.CalcularPaginacion( busqueda.Count());
            viewModel.Gramatica = gramatica;
            viewModel.listadoVocabularioIncluidos = gramatica.Vocabularios.Select(v => v.VocabularioId).ToList();

            int skip = (viewModel.Pagina - 1) * viewModel.resultadosPorPagina;
            viewModel.listadoVocabularios = busqueda.Skip(skip).Take(viewModel.resultadosPorPagina).ToList();
            return View(viewModel);
        }


        public void Add(int vocabularioId, int gramaticaId)
        {

            Gramatica gramatica = db.Gramaticas.Include(gr => gr.Vocabularios).FirstOrDefault(gr => gr.GramaticaId == gramaticaId);
            if (gramatica != null)
            {
                Vocabulario vocabulario = db.Vocabularios.Find(vocabularioId);
                if (vocabulario != null)
                {
                    gramatica.Vocabularios.Add(vocabulario);
                    db.Entry(gramatica).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }


        public void Remove(int vocabularioId, int gramaticaId)
        {
            Gramatica gramatica = db.Gramaticas.Include(gr => gr.Vocabularios).FirstOrDefault(gr => gr.GramaticaId == gramaticaId);
            if (gramatica != null)
            {
                Vocabulario vocabulario = db.Vocabularios.Find(vocabularioId);
                if (vocabulario != null)
                {
                    gramatica.Vocabularios.Remove(vocabulario);
                    db.Entry(gramatica).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }

        // GET: Admin/Vocabularios/Delete/5
        public ActionResult Delete(int vocabularioId, int gramaticaId)
        {
            if (gramaticaId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gramatica gramatica = db.Gramaticas.Include(gr=> gr.Vocabularios).FirstOrDefault(gr => gr.GramaticaId==gramaticaId);
            if (gramatica == null)
            {
                return HttpNotFound();
            }
            Vocabulario vocabulario = db.Vocabularios.Find(vocabularioId);
            if (vocabulario == null)
            {
                return HttpNotFound();
            }
            gramatica.Vocabularios.Remove(vocabulario);
            db.Entry(gramatica).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index", new {id=gramaticaId });
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
