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
using EnglishForCoachesWeb.Helpers;
using EnglishForCoachesWeb.Database.Varios;

namespace EnglishForCoachesWeb.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DocumentosController : MVCBaseController
    {
        private AuthContext db = new AuthContext();


        // GET: Admin/Documentos
        public ActionResult Index()
        {
            DocumentoIndexViewModel viewModel = new DocumentoIndexViewModel();
            var busqueda = db.Documentos.Include(Documento => Documento.Tema).Include(Documento=>Documento.SubTema).Include(Not=>Not.DocumentoGrupos.Select(gr=>gr.GrupoUsuario)).OrderByDescending(not=>not.DocumentoId).ToList();
            viewModel.Pagina = 1;

            viewModel.CalcularPaginacion( busqueda.Count());
            viewModel.listadoDocumentos = busqueda.Take(viewModel.resultadosPorPagina).ToList();
            return View(viewModel);
        }



        // POST: Admin/Documentos/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(DocumentoIndexViewModel viewModel)
        {
            var busqueda = db.Documentos.Include(Documento => Documento.Tema).Include(Documento => Documento.SubTema).Include(Not => Not.DocumentoGrupos.Select(gr => gr.GrupoUsuario)).OrderByDescending(not => not.DocumentoId).ToList();

            if (!string.IsNullOrWhiteSpace(viewModel.TextoBusqueda))
            {
                busqueda = busqueda.Where(x => x.Titulo.Contains(viewModel.TextoBusqueda)).ToList();
            }

            viewModel.CalcularPaginacion( busqueda.Count());

            int skip = (viewModel.Pagina - 1) * viewModel.resultadosPorPagina;
            viewModel.listadoDocumentos = busqueda.Skip(skip).Take(viewModel.resultadosPorPagina).ToList();
            return View(viewModel);
        }

        // GET: Admin/Documentos/Create
        public ActionResult Create( )
        {            DocumentoCreateViewModel viewModel = new DocumentoCreateViewModel();

            viewModel.InicializarDesplegables();
            return View(viewModel);
        }

        // POST: Admin/Documentos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DocumentoCreateViewModel viewModel)
        {
            //fichero pdf
            //"~/media/upload/Documentos_puntos/"

            if (ModelState.IsValid)
            {
                viewModel.Documento.Fecha = DateTime.Now;
                if (viewModel.Documento.TemaId.Value == 0)
                {
                    viewModel.Documento.TemaId = null;
                }
                if (viewModel.Documento.SubTemaId.Value==0)
                {
                    viewModel.Documento.SubTemaId = null;
                }
                db.Documentos.Add(viewModel.Documento);
                db.SaveChanges();
                if (viewModel.File != null)
                {
                    viewModel.Documento.FicheroAdjunto = viewModel.Documento.DocumentoId +"_"+ viewModel.File.FileName;

                    string nameAndLocation = "~/media/upload/documentos/" + viewModel.Documento.FicheroAdjunto;
                    viewModel.File.SaveAs(Server.MapPath(nameAndLocation));

                    db.Entry(viewModel.Documento).State = EntityState.Modified;
                    db.SaveChanges();
                }
              

                foreach (var grupo in viewModel.GruposUsuarios)
                {
                    if (grupo.Seleccionado)
                    {
                        DocumentoGrupo DocumentoGrupo = new DocumentoGrupo()
                        {
                            GrupoUsuarioId = grupo.Id,
                            DocumentoId = viewModel.Documento.DocumentoId
                        };
                        db.DocumentoGrupos.Add(DocumentoGrupo);
                        db.SaveChanges();
                    }
                }

                return RedirectToAction("Index",new { });
            }
            viewModel.InicializarDesplegables();
            return View(viewModel);
        }

        // GET: Admin/Documentos/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Documento Documento = db.Documentos.Include(not => not.DocumentoGrupos).FirstOrDefault(a => a.DocumentoId == id);
            if (Documento == null)
            {
                return HttpNotFound();
            }


            DocumentoEditViewModel viewModel = new DocumentoEditViewModel();

            viewModel.InicializarDesplegables();
            viewModel.Documento = Documento;
            foreach (var grupo in viewModel.GruposUsuarios)
            {
                if (Documento.DocumentoGrupos.FirstOrDefault(gr => gr.GrupoUsuarioId == grupo.Id) != null)
                {
                    grupo.Seleccionado = true;
                }
            }

            return View(viewModel);
        }

        // POST: Admin/Documentos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DocumentoEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (viewModel.Documento.TemaId.Value == 0)
                {
                    viewModel.Documento.TemaId = null;
                }

                if (viewModel.Documento.SubTemaId.Value == 0)
                {
                    viewModel.Documento.SubTemaId = null;
                }
                if (viewModel.File != null)
                {
                    string fullPath = Request.MapPath("~/media/upload/documentos/" + viewModel.Documento.FicheroAdjunto);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                    viewModel.Documento.FicheroAdjunto = viewModel.Documento.DocumentoId + "_" + viewModel.File.FileName;

                    string nameAndLocation = "~/media/upload/documentos/" + viewModel.Documento.FicheroAdjunto;
                    viewModel.File.SaveAs(Server.MapPath(nameAndLocation));
                }

               
                db.Entry(viewModel.Documento).State = EntityState.Modified;
                db.SaveChanges();

                foreach (var grupo in viewModel.GruposUsuarios)
                {
                    var existe = db.DocumentoGrupos.FirstOrDefault(gr=>gr.DocumentoId== viewModel.Documento.DocumentoId && gr.GrupoUsuarioId==grupo.Id);

                    if (grupo.Seleccionado&& existe==null)
                    {
                        DocumentoGrupo DocumentoGrupo = new DocumentoGrupo()
                        {
                            GrupoUsuarioId = grupo.Id,
                            DocumentoId = viewModel.Documento.DocumentoId
                        };
                        db.DocumentoGrupos.Add(DocumentoGrupo);
                        db.SaveChanges();
                    }
                    if (!grupo.Seleccionado && existe != null)
                    {
                        db.DocumentoGrupos.Remove(existe);
                        db.SaveChanges();
                    }
                }

                return RedirectToAction("Index", new {});
            }

            viewModel.InicializarDesplegables();
            return View(viewModel);
        }

        // GET: Admin/Documentos/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Documento Documento = db.Documentos.Find(id);
            if (Documento == null)
            {
                return HttpNotFound();
            }
                 

            string fullPath = Request.MapPath("~/media/upload/documentos/" + Documento.FicheroAdjunto);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
           
            //Eliminar subtema
            db.Documentos.Remove(Documento);
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
