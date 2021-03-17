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
    public class VocabulariosController : MVCBaseController
    {
        private AuthContext db = new AuthContext();


        // GET: Admin/Vocabularios
        public ActionResult Index()
        {
            VocabularioIndexViewModel viewModel = new VocabularioIndexViewModel();
            viewModel.Pagina = 1;
            var busqueda = db.Vocabularios.OrderBy(au => au.Palabra_en);
            viewModel.CalcularPaginacion( busqueda.Count());
            viewModel.listadoVocabularios = busqueda.Take(viewModel.resultadosPorPagina).ToList();
            return View(viewModel);
        }

    

        // POST: Admin/AccesoUsuarios/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(VocabularioIndexViewModel viewModel)
        {
            var busqueda = db.Vocabularios.OrderBy(au => au.Palabra_en).ToList();

            if (!string.IsNullOrWhiteSpace(viewModel.TextoBusqueda))
            {
                busqueda = busqueda.Where(x => x.Palabra_es.ToLower().Contains(viewModel.TextoBusqueda.ToLower()) || x.Palabra_en.ToLower().Contains(viewModel.TextoBusqueda.ToLower())).ToList();
            }

            viewModel.CalcularPaginacion( busqueda.Count());

            int skip = (viewModel.Pagina - 1) * viewModel.resultadosPorPagina;
            viewModel.listadoVocabularios = busqueda.Skip(skip).Take(viewModel.resultadosPorPagina).ToList();
            return View(viewModel);
        }

        // GET: Admin/Vocabularios/Create
        public ActionResult Create( )
        {            VocabularioCreateViewModel viewModel = new VocabularioCreateViewModel();
            viewModel.InicializarDesplegables();

            return View(viewModel);
        }

        // POST: Admin/Vocabularios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VocabularioCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                db.Vocabularios.Add(viewModel.Vocabulario);
                db.SaveChanges();
                if (viewModel.AudioFile != null)
                {
                    viewModel.Vocabulario.FicheroAudio = viewModel.Vocabulario.VocabularioId + ".mp3";

                    string nameAndLocation = "~/media/upload/audio/" + viewModel.Vocabulario.FicheroAudio;
                    viewModel.AudioFile.SaveAs(Server.MapPath(nameAndLocation));

                    db.Entry(viewModel.Vocabulario).State = EntityState.Modified;
                    db.SaveChanges();
                }

                return RedirectToAction("Index",new { });
            }
            viewModel.InicializarDesplegables();
            return View(viewModel);
        }

        // GET: Admin/Vocabularios/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vocabulario Vocabulario = db.Vocabularios.Include(a=>a.DefinicionVocabularios).FirstOrDefault(a => a.VocabularioId == id);
            if (Vocabulario == null)
            {
                return HttpNotFound();
            }


            VocabularioEditViewModel viewModel = new VocabularioEditViewModel();
            viewModel.DefinicionesVocabulario = "";
            foreach (var def in Vocabulario.DefinicionVocabularios)
            {
                viewModel.DefinicionesVocabulario += def.Palabra_es + "_" + def.Definicion + "#";
            }
            if(viewModel.DefinicionesVocabulario.Length>0)
            {
                viewModel.DefinicionesVocabulario=viewModel.DefinicionesVocabulario.Substring(0, viewModel.DefinicionesVocabulario.Length - 1);
            }

            viewModel.Vocabulario = Vocabulario;
            viewModel.InicializarDesplegables();
            return View(viewModel);
        }

        // POST: Admin/Vocabularios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VocabularioEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {

                if (viewModel.AudioFile != null)
                {
                    string fullPath = Request.MapPath("~/media/upload/audio/" + viewModel.Vocabulario.FicheroAudio);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                    viewModel.Vocabulario.FicheroAudio = viewModel.Vocabulario.VocabularioId + ".mp3";

                    string nameAndLocation = "~/media/upload/audio/" + viewModel.Vocabulario.FicheroAudio;
                    viewModel.AudioFile.SaveAs(Server.MapPath(nameAndLocation));
                }



                db.Entry(viewModel.Vocabulario).State = EntityState.Modified;
                db.SaveChanges();

                Vocabulario Vocabulario = db.Vocabularios.Include(a => a.DefinicionVocabularios).FirstOrDefault(a => a.VocabularioId == viewModel.Vocabulario.VocabularioId);

                List<DefinicionVocabulario> listaNueva = new List<DefinicionVocabulario>();
                
                if (!string.IsNullOrEmpty(viewModel.DefinicionesVocabulario))
                {
                    var definiciones = viewModel.DefinicionesVocabulario.Split('#');
                    foreach (var palDef in definiciones)
                    {
                        var arrPalDef = palDef.Split('_');

                        var existe=Vocabulario.DefinicionVocabularios.FirstOrDefault(dv=>dv.Palabra_es== arrPalDef[0] && dv.Definicion == arrPalDef[1]);
                        if (existe == null)
                        {
                            DefinicionVocabulario defVoc = new DefinicionVocabulario()
                            {
                                Palabra_es = arrPalDef[0],
                                Definicion = arrPalDef[1],
                                VocabularioId = Vocabulario.VocabularioId
                            };
                            listaNueva.Add(defVoc);
                        }
                        else
                        {
                            listaNueva.Add(existe);
                        }
                    }
                }

                List<string> listaNuevos = listaNueva.Select(ln=>ln.Palabra_es + "_" + ln.Definicion).ToList();

                var definicionQuitar=Vocabulario.DefinicionVocabularios.Where(dv => !listaNuevos.Contains(dv.Palabra_es + "_" + dv.Definicion));
                List<string> listaExisten = Vocabulario.DefinicionVocabularios.Select(ln => ln.Palabra_es + "_" + ln.Definicion).ToList();
                listaNueva.RemoveAll(dv => listaExisten.Contains(dv.Palabra_es + "_" + dv.Definicion));

                db.DefinicionVocabularios.RemoveRange(definicionQuitar);
                db.DefinicionVocabularios.AddRange(listaNueva);

                db.SaveChanges();


                return RedirectToAction("Index", new {});
            }

            viewModel.InicializarDesplegables();
            return View(viewModel);
        }

        // GET: Admin/Vocabularios/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vocabulario Vocabulario = db.Vocabularios.Find(id);
            if (Vocabulario == null)
            {
                return HttpNotFound();
            }
                 

            string fullPath = Request.MapPath("~/media/upload/audio/" + Vocabulario.FicheroAudio);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
            //Eliminar subtema
            db.Vocabularios.Remove(Vocabulario);
            db.SaveChanges();
            return RedirectToAction("Index", new { });
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
