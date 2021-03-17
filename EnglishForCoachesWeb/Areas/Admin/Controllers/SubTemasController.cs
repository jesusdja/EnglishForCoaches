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

namespace EnglishForCoachesWeb.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SubTemasController : MVCBaseController
    {
        private AuthContext db = new AuthContext();
        
        //Para el menu
        public ActionResult ListarTemas()
        {
            var temas = db.Temas.OrderBy(d=>d.TemaId);
            return PartialView("~/Areas/Admin/Views/Shared/_ListarTemas.cshtml", temas.ToList());
        }
        public ActionResult ListarTemasAlumno()
        {
            var temas = db.Temas.OrderBy(d => d.TemaId);
            return PartialView("~/Areas/Alumno/Views/Shared/_ListarTemasAlumno.cshtml", temas.ToList());
        }

        // GET: Admin/SubTemas
        public ActionResult Index(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var subTemas = db.SubTemas.Include(s => s.Tema).Where(subtema => subtema.TemaId == id).OrderBy(d => d.Orden).ToList();
            if (subTemas == null)
            {
                return HttpNotFound();
            }


            SubTemasIndexViewModel viewModel = new SubTemasIndexViewModel();
            viewModel.listadoSubTemas = subTemas;
            viewModel.Tema = db.Temas.Find(id);
            return View(viewModel);
        }

        //public void Importar()
        //{
            //ImportacionHelper import = new ImportacionHelper();
           // import.importarTest();
        //}

        // GET: Admin/SubTemas/Create
        public ActionResult Create(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tema tema = db.Temas.Where(a => a.TemaId == id).FirstOrDefault();
            if (tema == null)
            {
                return HttpNotFound();
            }


            SubTemasCreateViewModel viewModel = new SubTemasCreateViewModel();
            viewModel.Tema = db.Temas.Find(id);
            viewModel.CargarClienteSeleccionado(db);

            return View(viewModel);
        }

        // POST: Admin/SubTemas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SubTemasCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                viewModel.SubTema.TemaId = viewModel.Tema.TemaId;
                viewModel.SubTema.Orden = db.SubTemas.Count(subtema => subtema.TemaId == viewModel.Tema.TemaId)+1;
                db.SubTemas.Add(viewModel.SubTema);
                db.SaveChanges();

                AccesoClientesHelper.AnyadirSubTemaConHijos(viewModel.SubTema.SubTemaId, viewModel.Clientes.Where(cli => cli.Selected)
                    .Select(cli => Convert.ToInt32(cli.Value)).ToList());

                return RedirectToAction("Index",new {id=viewModel.Tema.TemaId });
            }
            viewModel.Tema = db.Temas.Find(viewModel.Tema.TemaId);
            return View(viewModel);
        }

        // GET: Admin/SubTemas/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubTema subtema = db.SubTemas.Where(a => a.SubTemaId == id).FirstOrDefault();
            if (subtema == null)
            {
                return HttpNotFound();
            }


            SubTemasEditViewModel viewModel = new SubTemasEditViewModel();
            viewModel.Tema = db.Temas.Find(subtema.TemaId);
            viewModel.SubTema = subtema;
            viewModel.CargarClienteSeleccionado(db);
            return View(viewModel);
        }

        // POST: Admin/SubTemas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SubTemasEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(viewModel.SubTema).State = EntityState.Modified;
                db.SaveChanges();
                AccesoClientesHelper.AnyadirSubTemaConHijos(viewModel.SubTema.SubTemaId, viewModel.Clientes.Where(cli => cli.Selected)
                    .Select(cli => Convert.ToInt32(cli.Value)).ToList());

                return RedirectToAction("Index", new { id = viewModel.SubTema.TemaId });
            }

            viewModel.Tema = db.Temas.Find(viewModel.Tema.TemaId);
            
            return View(viewModel);
        }

        // GET: Admin/SubTemas/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubTema subTema = db.SubTemas.Find(id);
            if (subTema == null)
            {
                return HttpNotFound();
            }

            //Ordenar de nuevo todos los subtemas            
            foreach (SubTema st in db.SubTemas.Where(b => b.TemaId == subTema.TemaId && b.Orden >= subTema.Orden).ToList())
            {
                st.Orden = (st.Orden) - 1;

                db.Entry(st).State = EntityState.Modified;
            }
            db.SaveChanges();
                            
            //Eliminar subtema
            db.SubTemas.Remove(subTema);
            db.SaveChanges();
            return RedirectToAction("Index", new { id = subTema.TemaId});
        }


        // GET: Admin/SubTema/Move
        public ActionResult Move(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var subtema = db.SubTemas.Find(id);
            if (subtema == null)
            {
                return HttpNotFound();
            }

            SubTemasMoveViewModel viewModel = new SubTemasMoveViewModel();
            viewModel.SubTema = subtema;
            viewModel.InicializarDesplegables();

            return View(viewModel);
        }

        // POST: Admin/SubTema/Move
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Move(SubTemasMoveViewModel viewModel)
        {
            int id = viewModel.SubTema.SubTemaId;
            var subtema = db.SubTemas.Find(id);
            viewModel.SubTema = subtema;
            if (ModelState.IsValid)
            {
                viewModel.SubTema.TemaId = viewModel.TemaCopiarId;


                db.Entry(viewModel.SubTema).State = EntityState.Modified;
                db.SaveChanges();

                recalcularOrden(viewModel.SubTema.TemaId);
                return RedirectToAction("Index", "SubTemas", new { id = viewModel.SubTema.TemaId });
            }

            viewModel.InicializarDesplegables();
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


        //POST: ModificarOrden
        public ActionResult ModificarOrden(int id, string posicion)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubTema subTema = db.SubTemas.Find(id);
            if (subTema == null)
            {
                return HttpNotFound();
            }

            actualizarOrdenSubtema(id, 1, posicion);
            

            return RedirectToAction("Index", new { id = subTema.TemaId });
        }

        private void recalcularOrden(int temaId)
        {
            var subtemas = db.SubTemas.Where(b => b.TemaId == temaId).OrderBy(ex => ex.Orden).ToArray();
            Int32 ordenNuevo = 1;
            for (var i = 0; i < subtemas.Length; i++)
            {
                subtemas[i].Orden = ordenNuevo;
                db.Entry(subtemas[i]).State = EntityState.Modified;
                db.SaveChanges();

                ordenNuevo++;
            }
        }

        private void actualizarOrdenSubtema(int id, int posicion, string mov)
        {
            SubTema subTema = db.SubTemas.Find(id);
            Int32 ordenNuevo;

            if (mov.Equals("top"))
            {
                //Al subtema seleccionado le restamos 1 posicion
                ordenNuevo = (subTema.Orden) - 1;

                //Buscamos el subtema con el orden a modificar para sumarle 1 posicion
                SubTema st= db.SubTemas.Where(b=>b.TemaId==subTema.TemaId && b.Orden == ordenNuevo).FirstOrDefault();
                st.Orden = (st.Orden) + 1;
                db.Entry(st).State = EntityState.Modified;
                db.SaveChanges();

                subTema.Orden = ordenNuevo;
                db.Entry(subTema).State = EntityState.Modified;
                db.SaveChanges();   
            }

            if (mov.Equals("down"))
            {
                //Al subtema seleccionado le sumamos 1 posicion
                ordenNuevo = (subTema.Orden) + 1;

                //Buscamos el subtema con el orden a modificar para restarle 1 posicion
                SubTema st = db.SubTemas.Where(b => b.TemaId == subTema.TemaId && b.Orden == ordenNuevo).FirstOrDefault();
                st.Orden = (st.Orden) - 1;
                db.Entry(st).State = EntityState.Modified;
                db.SaveChanges();

                subTema.Orden = ordenNuevo;
                db.Entry(subTema).State = EntityState.Modified;
                db.SaveChanges();
            }

        }
    }
}
