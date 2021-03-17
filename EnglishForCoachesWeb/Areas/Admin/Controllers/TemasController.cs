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
using EnglishForCoachesWeb.Database.Clientes;

namespace EnglishForCoachesWeb.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TemasController : MVCBaseController
    {
        private AuthContext db = new AuthContext();

        // GET: Admin/Temas
        public ActionResult Index()
        {
            TemaIndexViewModel viewModel = new TemaIndexViewModel();
            viewModel.Pagina = 1;
            var busqueda = db.Temas.OrderBy(au => au.Descripcion);
            viewModel.CalcularPaginacion(busqueda.Count());
            viewModel.listadoTemas = busqueda.Take(viewModel.resultadosPorPagina).ToList();
            return View(viewModel);
        }



        // POST: Admin/AccesoUsuarios/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(TemaIndexViewModel viewModel)
        {
            var busqueda = db.Temas.OrderBy(au => au.Descripcion).ToList();

            if (!string.IsNullOrWhiteSpace(viewModel.TextoBusqueda))
            {
                busqueda = busqueda.Where(x =>  x.Descripcion.ToLower().Contains(viewModel.TextoBusqueda.ToLower())).ToList();
            }

            viewModel.CalcularPaginacion(busqueda.Count());

            int skip = (viewModel.Pagina - 1) * viewModel.resultadosPorPagina;
            viewModel.listadoTemas = busqueda.Skip(skip).Take(viewModel.resultadosPorPagina).ToList();
            return View(viewModel);
        }

        // GET: Admin/Temas/Create
        public ActionResult Create()
        {
            TemaCreateViewModel viewModel = new TemaCreateViewModel();

            viewModel.CargarClienteSeleccionado(db);
            return View(viewModel);
        }

        // POST: Admin/Temas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TemaCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {

                    db.Temas.Add(viewModel.Tema);
                    db.SaveChanges();


                AccesoClientesHelper.AnyadirTemaConHijos(viewModel.Tema.TemaId,viewModel.Clientes.Where(cli=>cli.Selected)
                    .Select(cli=>Convert.ToInt32(cli.Value)).ToList());

                return RedirectToAction("Index", new { });
                
            }
            return View(viewModel);
        }

        // GET: Admin/Temas/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tema Tema = db.Temas.FirstOrDefault(a => a.TemaId == id);
            if (Tema == null)
            {
                return HttpNotFound();
            }


            TemaEditViewModel viewModel = new TemaEditViewModel();

            viewModel.Tema = Tema;
            viewModel.CargarClienteSeleccionado(db);
            return View(viewModel);
        }

        // POST: Admin/Temas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TemaEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                    db.Entry(viewModel.Tema).State = EntityState.Modified;
                    db.SaveChanges();



                AccesoClientesHelper.AnyadirTemaConHijos(viewModel.Tema.TemaId, viewModel.Clientes.Where(cli => cli.Selected)
                    .Select(cli => Convert.ToInt32(cli.Value)).ToList());

                return RedirectToAction("Index", new { });
                
            }

            return View(viewModel);
        }

        // GET: Admin/Temas/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tema Tema = db.Temas.Find(id);
            if (Tema == null)
            {
                return HttpNotFound();
            }
            var nSubtemas = db.SubTemas.Count(sub => sub.TemaId == id);

            if (nSubtemas == 0)
            {
                //Eliminar 
                db.Temas.Remove(Tema);
                db.SaveChanges();
            }
            return RedirectToAction("Index", new { });
        }



        // GET: Admin/Temas/Video/5
        public ActionResult Video(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tema Tema = db.Temas.Where(a => a.TemaId == id).FirstOrDefault();
            if (Tema == null)
            {
                return HttpNotFound();
            }


            TemasVideoViewModel viewModel = new TemasVideoViewModel();
            viewModel.Tema = db.Temas.Find(Tema.TemaId);
            viewModel.Tema = Tema;
            return View(viewModel);
        }

        // POST: Admin/Temas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Video(TemasVideoViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(viewModel.Tema).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "SubTemas",new { id = viewModel.Tema.TemaId });
            }

            viewModel.Tema = db.Temas.Find(viewModel.Tema.TemaId);
            
            return View(viewModel);
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
