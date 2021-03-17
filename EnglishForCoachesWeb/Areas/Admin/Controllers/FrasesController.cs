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
    public class FrasesController : MVCBaseController
    {
        private AuthContext db = new AuthContext();

        // GET: Admin/Frases
        public ActionResult Index(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var gramatica = db.Gramaticas.Include(s => s.SubTema.Tema).SingleOrDefault(s => s.GramaticaId == id);
            if (gramatica == null)
            {
                return HttpNotFound();
            }
            
            var Frases = db.Frases.Where(t => t.GramaticaId == id).ToList();


            FrasesIndexViewModel viewModel = new FrasesIndexViewModel();
            viewModel.listadoFrases = Frases;
            viewModel.Gramatica = gramatica;
            return View(viewModel);
        }

        // GET: Admin/Frases/Gestor
        public ActionResult Gestor(string textoBusqueda, int? temaId, int? subtemaId, int? gramaticaId,int? pagina)
        {
            FrasesGestorViewModel viewModel = new FrasesGestorViewModel();
            viewModel.Pagina = 1;
            if(pagina.HasValue)
            {
                viewModel.Pagina = pagina.Value;
            }

            var Frases = buscar(textoBusqueda,  temaId,  subtemaId,  gramaticaId);


            viewModel.CalcularPaginacion(Frases.Count());
            int skip = (viewModel.Pagina - 1) * viewModel.resultadosPorPagina;
            viewModel.listadoFrases = Frases.OrderBy(au => au.Palabra_es).Skip(skip).Take(viewModel.resultadosPorPagina).ToList();
            viewModel.InicializarDesplegables();
            return View(viewModel);
        }

        // POST: Admin/Frases/Gestor/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Gestor(FrasesGestorViewModel viewModel)
        {
            var busqueda = buscar(viewModel.TextoBusqueda, viewModel.TemaId, viewModel.SubtemaId, viewModel.GramaticaId);


            viewModel.CalcularPaginacion(busqueda.Count());

            int skip = (viewModel.Pagina - 1) * viewModel.resultadosPorPagina;
            viewModel.listadoFrases = busqueda.OrderBy(au => au.Palabra_es).Skip(skip).Take(viewModel.resultadosPorPagina).ToList();
            viewModel.InicializarDesplegables();
            return View(viewModel);
        }

        private IQueryable<Frase> buscar(string textoBusqueda, int? temaId, int? subtemaId, int? gramaticaId)
        {
            var busqueda = db.Frases.Include(fr => fr.Gramatica.SubTema.Tema).Include(fr => fr.Vocabularios);

            if (!string.IsNullOrWhiteSpace(textoBusqueda))
            {
                busqueda = busqueda.Where(x => x.Palabra_es.ToLower().Contains(textoBusqueda.ToLower()) || x.Palabra_en.ToLower().Contains(textoBusqueda.ToLower()) 
                || x.Vocabularios.Any(vo => vo.Palabra_en.Contains(textoBusqueda.ToLower())));
            }
            if (temaId.HasValue)
            {
                busqueda = busqueda.Where(x => x.Gramatica.SubTema.TemaId == temaId.Value);
            }
            if (subtemaId.HasValue)
            {
                busqueda = busqueda.Where(x => x.Gramatica.SubTemaId == subtemaId.Value);
            }
            if (gramaticaId.HasValue)
            {
                busqueda = busqueda.Where(x => x.GramaticaId == gramaticaId.Value);
            }
            return busqueda;
        }



        // GET: Admin/Frases/Create
        public ActionResult Create(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var gramatica = db.Gramaticas.Include(s => s.SubTema.Tema).SingleOrDefault(s => s.GramaticaId == id);
            if (gramatica == null)
            {
                return HttpNotFound();
            }


            FrasesCreateViewModel viewModel = new FrasesCreateViewModel();
            viewModel.GramaticaId = gramatica.GramaticaId;
            viewModel.GramaticaTitulo = gramatica.Titulo;
            viewModel.SubTema = gramatica.SubTema;

            return View(viewModel);
        }


        // POST: Admin/Frases/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FrasesCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
               
                    viewModel.Frase.GramaticaId = viewModel.GramaticaId;
                    db.Frases.Add(viewModel.Frase);
                    db.SaveChanges();


                if (viewModel.AudioFile_es != null)
                {
                    viewModel.Frase.FicheroAudio_es = viewModel.Frase.FraseId + "_es.mp3";

                    string nameAndLocation = "~/media/upload/frases_mp3/" + viewModel.Frase.FicheroAudio_es;
                    viewModel.AudioFile_es.SaveAs(Server.MapPath(nameAndLocation));

                    db.Entry(viewModel.Frase).State = EntityState.Modified;
                    db.SaveChanges();
                }
                if (viewModel.AudioFile_en != null)
                {
                    viewModel.Frase.FicheroAudio_en = viewModel.Frase.FraseId + "_en.mp3";

                    string nameAndLocation = "~/media/upload/frases_mp3/" + viewModel.Frase.FicheroAudio_en;
                    viewModel.AudioFile_en.SaveAs(Server.MapPath(nameAndLocation));

                    db.Entry(viewModel.Frase).State = EntityState.Modified;
                    db.SaveChanges();
                }

                return RedirectToAction("Index", "Frases", new { id = viewModel.Frase.GramaticaId });
                
            }

            var gramatica = db.Gramaticas.Include(s => s.SubTema.Tema).SingleOrDefault(s => s.GramaticaId == viewModel.GramaticaId);
            viewModel.SubTema = gramatica.SubTema;
            return View(viewModel);
        }


        // GET: Admin/Frases/Edit/5
        public ActionResult Edit(int id,string src, string textoBusqueda, int? temaId, int? subtemaId, int? gramaticaId, int? Pagina)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Frase Frase = db.Frases.Include(fr=>fr.Vocabularios).FirstOrDefault(a => a.FraseId == id);
            if (Frase == null)
            {
                return HttpNotFound();
            }


            FrasesEditViewModel viewModel = new FrasesEditViewModel();
            var gramatica = db.Gramaticas.Include(s => s.SubTema.Tema).SingleOrDefault(s => s.GramaticaId == Frase.GramaticaId);

            viewModel.GramaticaId = gramatica.GramaticaId;
            viewModel.GramaticaTitulo = gramatica.Titulo;
            viewModel.Frase = Frase;
            viewModel.SubTema = gramatica.SubTema;
            viewModel.src = src;
            viewModel.InicializarDesplegables();

            //Mantener busqueda
            viewModel.TextoBusqueda = textoBusqueda;
            viewModel.TemaIdBusqueda = temaId;
            viewModel.SubtemaIdBusqueda = subtemaId;
            viewModel.GramaticaIdBusqueda = gramaticaId;
            viewModel.PaginaBusqueda = Pagina;



            viewModel.VocabularioIds= string.Join("#", Frase.Vocabularios.Select(f=>f.VocabularioId).ToArray());
            return View(viewModel);
        }

        // POST: Admin/Frases/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FrasesEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {

                if (viewModel.AudioFile_es != null)
                {
                    string fullPath = Request.MapPath("~/media/upload/frases_mp3/" + viewModel.Frase.FicheroAudio_es);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                    viewModel.Frase.FicheroAudio_es = viewModel.Frase.FraseId + "_es.mp3";

                    string nameAndLocation = "~/media/upload/frases_mp3/" + viewModel.Frase.FicheroAudio_es;
                    viewModel.AudioFile_es.SaveAs(Server.MapPath(nameAndLocation));

                    db.Entry(viewModel.Frase).State = EntityState.Modified;
                    db.SaveChanges();
                }
                if (viewModel.AudioFile_en != null)
                {
                    string fullPath = Request.MapPath("~/media/upload/frases_mp3/" + viewModel.Frase.FicheroAudio_en);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                    viewModel.Frase.FicheroAudio_en = viewModel.Frase.FraseId + "_en.mp3";

                    string nameAndLocation = "~/media/upload/frases_mp3/" + viewModel.Frase.FicheroAudio_en;
                    viewModel.AudioFile_en.SaveAs(Server.MapPath(nameAndLocation));

                    db.Entry(viewModel.Frase).State = EntityState.Modified;
                    db.SaveChanges();
                }

                db.Entry(viewModel.Frase).State = EntityState.Modified;
                db.SaveChanges();

                Frase Frase = db.Frases.Include(fr => fr.Vocabularios).FirstOrDefault(a => a.FraseId == viewModel.Frase.FraseId);
                var vocabulariosFrase = Frase.Vocabularios.ToList();
                foreach (var vocabulario in vocabulariosFrase)
                {
                    Frase.Vocabularios.Remove(vocabulario);
                }
         

                //VocabularioIds
                if (!string.IsNullOrWhiteSpace(viewModel.VocabularioIds))
                {
                    if (viewModel.VocabularioIds.StartsWith("#"))
                    {
                        viewModel.VocabularioIds = viewModel.VocabularioIds.Substring(1, viewModel.VocabularioIds.Length - 1);
                    }
                    var arrVocabularioIds = viewModel.VocabularioIds.Split('#');

                    foreach (var vocabularioId in arrVocabularioIds)
                    {
                        var vocabulario = db.Vocabularios.First(vo=>vo.VocabularioId.ToString()==vocabularioId);
                        if (!Frase.Vocabularios.Contains(vocabulario))
                        {
                            Frase.Vocabularios.Add(vocabulario);
                        }
                    }
                }
                db.Entry(Frase).State = EntityState.Modified;
                db.SaveChanges();

                if (viewModel.src == "Gestor")                {
                    return RedirectToAction("Gestor", "Frases",new { textoBusqueda = viewModel.TextoBusqueda, temaId = viewModel.TemaIdBusqueda,
                        subtemaId = viewModel.SubtemaIdBusqueda, gramaticaId = viewModel.GramaticaIdBusqueda, pagina = viewModel.PaginaBusqueda });
                }
                return RedirectToAction("Index", "Frases", new { id = viewModel.Frase.GramaticaId });
            }

            var gramatica = db.Gramaticas.Include(s => s.SubTema.Tema).SingleOrDefault(s => s.GramaticaId == viewModel.Frase.GramaticaId);
            viewModel.SubTema = gramatica.SubTema;
            viewModel.InicializarDesplegables();
            return View(viewModel);
        }

        // GET: Admin/Frases/Delete/5
        public ActionResult Delete(int? id,string src, string textoBusqueda, int? temaId, int? subtemaId, int? gramaticaId, int? Pagina)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Frase Frase = db.Frases.Find(id);
            if (Frase == null)
            {
                return HttpNotFound();
            }
            string fullPath = Request.MapPath("~/media/upload/frases_mp3/" + Frase.FicheroAudio_es);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
            fullPath = Request.MapPath("~/media/upload/frases_mp3/" + Frase.FicheroAudio_en);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }

            db.Frases.Remove(Frase);
            db.SaveChanges();
            if(src=="Gestor")
            {
                return RedirectToAction("Gestor", "Frases", new
                {
                    textoBusqueda = textoBusqueda,
                    temaId = temaId,
                    subtemaId = subtemaId,
                    gramaticaId = gramaticaId,
                    pagina = Pagina
                });
            }
            return RedirectToAction("Index", "Frases", new { id = Frase.GramaticaId });
        }

        // GET: Admin/Frases/Copy
        public ActionResult Copy(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var frase = db.Frases.Find(id);
            if (frase == null)
            {
                return HttpNotFound();
            }

            FrasesCopyViewModel viewModel = new FrasesCopyViewModel();
            viewModel.Frase = frase;
            viewModel.InicializarDesplegables();

            var gramatica = db.Gramaticas.Include(s => s.SubTema.Tema).SingleOrDefault(s => s.GramaticaId == frase.GramaticaId);

            viewModel.GramaticaId = gramatica.GramaticaId;
            viewModel.GramaticaTitulo = gramatica.Titulo;
            viewModel.SubTema = gramatica.SubTema;

            return View(viewModel);
        }

        // POST: Admin/Frases/Move
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Copy(FrasesCopyViewModel viewModel)
        {
            int id = viewModel.Frase.FraseId;
            var frase = db.Frases.Find(id);
            viewModel.Frase = frase;
            var gramatica = db.Gramaticas.Include(s => s.SubTema.Tema).SingleOrDefault(s => s.GramaticaId == frase.GramaticaId);

            viewModel.GramaticaId = gramatica.GramaticaId;
            viewModel.GramaticaTitulo = gramatica.Titulo;
            viewModel.SubTema = gramatica.SubTema;
            if (ModelState.IsValid)
            {
                viewModel.Frase.GramaticaId = viewModel.GramaticaCopiarId;

                db.Frases.Add(viewModel.Frase);
                db.SaveChanges();


                if (!string.IsNullOrEmpty(viewModel.Frase.FicheroAudio_en))
                {
                    string oldPathAndName = "~/media/upload/frases_mp3/" + viewModel.Frase.FicheroAudio_en;
                    string newPathAndName = "~/media/upload/frases_mp3/" + viewModel.Frase.FraseId + "_en.mp3";

                    System.IO.File.Copy(Server.MapPath(oldPathAndName), Server.MapPath(newPathAndName));

                    viewModel.Frase.FicheroAudio_en = viewModel.Frase.FraseId + "_en.mp3";
                    db.Entry(viewModel.Frase).State = EntityState.Modified;
                    db.SaveChanges();
                }
                if (!string.IsNullOrEmpty(viewModel.Frase.FicheroAudio_es))
                {
                    string oldPathAndName = "~/media/upload/frases_mp3/" + viewModel.Frase.FicheroAudio_es;
                    string newPathAndName = "~/media/upload/frases_mp3/" + viewModel.Frase.FraseId + "_es.mp3";

                    System.IO.File.Copy(Server.MapPath(oldPathAndName), Server.MapPath(newPathAndName));

                    viewModel.Frase.FicheroAudio_es = viewModel.Frase.FraseId + "_es.mp3";
                    db.Entry(viewModel.Frase).State = EntityState.Modified;
                    db.SaveChanges();
                }

                return RedirectToAction("Index", "Frases", new { id = viewModel.Frase.GramaticaId });
            }

            viewModel.InicializarDesplegables();
            return View(viewModel);
        }

        public ActionResult AsignarVocabulario(int FraseId, int VocabularioId)
        {
            if (FraseId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (VocabularioId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            var frase = db.Frases.Include(fr=>fr.Vocabularios).First(fr=>fr.FraseId==FraseId);
            var vocabulario = db.Vocabularios.Find(VocabularioId);
            if (!frase.Vocabularios.Contains(vocabulario))
            {
                frase.Vocabularios.Add(vocabulario);
                db.Entry(frase).State = EntityState.Modified;
                db.SaveChanges();
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public class MinVocabulario
        {
            public string Nombre_es { get; set; }
            public int Id { get; set; }
        }

        [HttpGet]
        public JsonResult GetVocabulario()
        {
            var vocabulario = db.Vocabularios.Select(vo => new MinVocabulario { Id = vo.VocabularioId, Nombre_es = vo.Palabra_en }).OrderBy(x=>x.Nombre_es).ToList();
       
            return Json(vocabulario, JsonRequestBehavior.AllowGet);
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
