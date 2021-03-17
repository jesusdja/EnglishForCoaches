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
using EnglishForCoachesWeb.Database.JuegoOnline;
using static EnglishForCoachesWeb.Areas.Admin.Controllers.FrasesController;

namespace EnglishForCoachesWeb.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SopaLetrasController : MVCBaseController
    {
        private AuthContext db = new AuthContext();

        // GET: Admin/SopaLetras/Create
        public ActionResult Create(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JuegoOnline juegoOnline = db.JuegoOnlines.SingleOrDefault(bl => bl.JuegoOnlineId == id);
            if (juegoOnline == null)
            {
                return HttpNotFound();
            }
            SopaLetrasCreateViewModel viewModel = new SopaLetrasCreateViewModel();
            viewModel.Inicializar(id);
            viewModel.SopaLetras = db.SopaLetras.Include(c=>c.CasillaSopaLetras).Include(c => c.VocabularioSopaLetras).FirstOrDefault(c => c.JuegoOnlineId == id);

            viewModel.Letras = new string[12][];
      
                for (int i = 0; i < 12; i++)
                {
                    viewModel.Letras[i] = new string[12];
                    for (int j = 0; j < 12; j++)
                    {
                        var casilla = viewModel.SopaLetras.CasillaSopaLetras.FirstOrDefault(cr => cr.PosH == j && cr.PosV == i);
                        if (casilla != null)
                        {

                            viewModel.Letras[i][j] = casilla.letra;
                        }
                    }
                }

                foreach(var comentario in viewModel.SopaLetras.VocabularioSopaLetras)
            {
                var vocabulario = db.Vocabularios.Find(comentario.VocabularioId);
                viewModel.Comentarios += comentario.VocabularioId + "|" + comentario.Comentario + "|" + vocabulario.Palabra_en + "#";
            }
            if(viewModel.Comentarios!=null)
            {
                viewModel.Comentarios = viewModel.Comentarios.Substring(0, viewModel.Comentarios.Length - 1);
            }
            return View(viewModel);
        }
        // POST: Admin/SopaLetras/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SopaLetrasCreateViewModel viewModel)
        {           
            if (ModelState.IsValid)
            {
                viewModel.SopaLetras.Descripcion = viewModel.SopaLetras.Enunciado;
                
                db.Entry(viewModel.SopaLetras).State = EntityState.Modified;
                db.SaveChanges();

                viewModel.SopaLetras = db.SopaLetras.Include(c => c.CasillaSopaLetras).Include(c => c.VocabularioSopaLetras).FirstOrDefault(c => c.Id == viewModel.SopaLetras.Id);
              
                if (!string.IsNullOrWhiteSpace(viewModel.Comentarios))
                {
                    var arrComentarios = viewModel.Comentarios.Split('#');
                    List<int> listaPalabras=new List<int>();
                    foreach (var comentario in arrComentarios)
                    {
                        var arrDatos = comentario.Split('|');
                        if(arrDatos.Length>1)
                        {
                            var vocaId = Convert.ToInt32(arrDatos[0]);
                            VocabularioSopaLetras comentarioSopaLetras = viewModel.SopaLetras.VocabularioSopaLetras.FirstOrDefault(cr => cr.VocabularioId== vocaId);
                            if (comentarioSopaLetras == null)
                            {
                                comentarioSopaLetras = new VocabularioSopaLetras()
                                {
                                    VocabularioId = vocaId,
                                    Comentario = arrDatos[1],
                                    SopaLetras= viewModel.SopaLetras
                                    
                                };
                                db.VocabularioSopaLetras.Add(comentarioSopaLetras);
                                db.SaveChanges();
                            }
                            else
                            {
                                comentarioSopaLetras.Comentario = arrDatos[1];
                                db.Entry(comentarioSopaLetras).State = EntityState.Modified;
                                db.SaveChanges();
                            }

                            listaPalabras.Add(vocaId);
                        }
                    }

                    viewModel.SopaLetras = db.SopaLetras.Include(c => c.CasillaSopaLetras).Include(c => c.VocabularioSopaLetras).FirstOrDefault(c => c.Id == viewModel.SopaLetras.Id);
                    if (listaPalabras.Count>0)
                    {
                        var comentariosEliminar= viewModel.SopaLetras.VocabularioSopaLetras.Where(cr =>!listaPalabras.Contains( cr.VocabularioId) ).ToList();
                        foreach (var comentario in comentariosEliminar)
                        {

                            db.VocabularioSopaLetras.Remove(comentario);
                            db.SaveChanges();
                        }
                    }
                    
                }


                return RedirectToAction("Index", "Bloques", new { id = viewModel.SopaLetras.SubTemaId, pestanya = (int)PestanyasBloques.JuegosOnline });
            }
            viewModel.SopaLetras = db.SopaLetras.Include(c => c.CasillaSopaLetras).Include(c => c.VocabularioSopaLetras).FirstOrDefault(c => c.JuegoOnlineId == viewModel.SopaLetras.JuegoOnlineId);
            viewModel.Inicializar(viewModel.SopaLetras.SubTemaId);
            return View(viewModel);
        }


        [HttpGet]
        public JsonResult GetVocabulario()
        {
            var vocabulario = db.Vocabularios.Where(vo=>vo.Palabra_en.Length<=12 ).Select(vo => new MinVocabulario { Id = vo.VocabularioId, Nombre_es = vo.Palabra_en.ToLower() }).OrderBy(x => x.Nombre_es).ToList();
            vocabulario = vocabulario.Where(vo => vo.Nombre_es.IndexOf(' ') < 0).ToList();
            return Json(vocabulario, JsonRequestBehavior.AllowGet);
        }

        // GET: Admin/SopaLetras/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SopaLetras SopaLetras = db.SopaLetras.Find(id);
            if (SopaLetras == null)
            {
                return HttpNotFound();
            }

            db.SopaLetras.Remove(SopaLetras);
            db.SaveChanges();
            return RedirectToAction("Create", "SopaLetras", new { id = SopaLetras.JuegoOnlineId });
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
