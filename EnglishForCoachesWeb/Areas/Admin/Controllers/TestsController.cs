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
    public class TestsController : MVCBaseController
    {
        private AuthContext db = new AuthContext();

        // GET: Admin/Tests/Create
        public ActionResult Create(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bloque bloque =db.Bloques.SingleOrDefault(bl => bl.BloqueId == id);
            if (bloque == null)
            {
                return HttpNotFound();
            }

            TestCreateViewModel viewModel = new TestCreateViewModel();
            viewModel.Inicializar(id);
            
            viewModel.Test = new Test();
            viewModel.Test.TipoEjercicioId = (int)TiposDeEjerciciosId.Test;
            viewModel.Test.BloqueId = id;
            viewModel.Test.SubTemaId = bloque.SubTemaId;
            viewModel.Test.AreaId = viewModel.bloque.AreaId;
            return View(viewModel);
        }
        
        // POST: Admin/Tests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TestCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                viewModel.Test.Descripcion = viewModel.Test.Enunciado;

                db.Tests.Add(viewModel.Test);
                db.SaveChanges();
                if (viewModel.ImageFile != null)
                {
                    viewModel.Test.UrlImagen = viewModel.Test.Id + ".jpg";

                    string nameAndLocation = "~/media/upload/Test/" + viewModel.Test.UrlImagen;
                    viewModel.ImageFile.SaveAs(Server.MapPath(nameAndLocation));

                    db.Entry(viewModel.Test).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Create", "Tests", new { id = viewModel.Test.BloqueId});
            }


            viewModel.Inicializar(viewModel.Test.BloqueId);
            return View(viewModel);
        }

        // GET: Admin/Tests/Edit/5
        public ActionResult Edit(int id,int? examenId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Test test = db.Tests.Find(id);
            if (test == null)
            {
                return HttpNotFound();
            }

            TestEditViewModel viewModel = new TestEditViewModel();
            viewModel.Inicializar(test.BloqueId);
            viewModel.ExamenId = examenId;
            viewModel.Test = test;
            return View(viewModel);
        }

        // POST: Admin/Tests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TestEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                viewModel.Test.Descripcion = viewModel.Test.Enunciado;
                

                if (viewModel.ImageFile != null)
                {
                    if (viewModel.Test.UrlImagen != null)
                    {
                        var fullPath = Request.MapPath("~/media/upload/Test/" + viewModel.Test.UrlImagen);
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }
                    }

                    viewModel.Test.UrlImagen = viewModel.Test.Id + ".jpg";

                    string nameAndLocation = "~/media/upload/Test/" + viewModel.Test.UrlImagen;
                    viewModel.ImageFile.SaveAs(Server.MapPath(nameAndLocation));
                }

                db.Entry(viewModel.Test).State = EntityState.Modified;
                db.SaveChanges();
                if (viewModel.ExamenId.HasValue) {
                    return RedirectToAction("Edit", "ErroresExamens", new { id = viewModel.ExamenId.Value });
                } else {
                    return RedirectToAction("Create", "Tests", new { id = viewModel.Test.BloqueId });
                }
            }
            viewModel.Inicializar(viewModel.Test.BloqueId);
            return View(viewModel);
        }

        // GET: Admin/Tests/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Test Test = db.Tests.Find(id);
            if (Test == null)
            {
                return HttpNotFound();
            }
            if (Test.UrlImagen != null)
            {
                var fullPath = Request.MapPath("~/media/upload/Test/" + Test.UrlImagen);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }

            db.Tests.Remove(Test);
            db.SaveChanges();
            return RedirectToAction("Create", "Tests", new { id = Test.BloqueId });
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

