using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.IO;
using EnglishForCoachesWeb.Database;
using EnglishForCoachesWeb.Database.Ejercicios;
using EnglishForCoachesWeb.Areas.Admin.Models;
using EnglishForCoachesWeb.Controllers;
using System.Text.RegularExpressions;
using EnglishForCoachesWeb.Helpers;

namespace EnglishForCoachesWeb.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class GramaticasController : MVCBaseController
    {
        private AuthContext db = new AuthContext();
        
        

        // GET: Admin/Gramaticas/Create
        public ActionResult Create(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubTema subTema = db.SubTemas.FirstOrDefault(a => a.SubTemaId == id);
            if (subTema == null)
            {
                return HttpNotFound();
            }


            GramaticasCreateViewModel viewModel = new GramaticasCreateViewModel();
            viewModel.SubTema = subTema;
            viewModel.Tema = db.Temas.Find(viewModel.SubTema.TemaId);
            viewModel.CargarClienteSeleccionado(db);

            return View(viewModel);
        }

        // POST: Admin/Gramaticas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GramaticasCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                viewModel.Gramatica.SubTemaId = viewModel.SubTema.SubTemaId;
                viewModel.Gramatica.Orden = db.Gramaticas.Count(grama => grama.SubTemaId == viewModel.SubTema.SubTemaId) +1;
                db.Gramaticas.Add(viewModel.Gramatica);
                db.SaveChanges();


                ActualizarCuerpo(viewModel.Gramatica);

                AccesoClientesHelper.AnyadirGramaticaConHijos(viewModel.Gramatica.GramaticaId, viewModel.Clientes.Where(cli => cli.Selected)
                    .Select(cli => Convert.ToInt32(cli.Value)).ToList());

                return RedirectToAction("Index","Bloques",new {id=viewModel.SubTema.SubTemaId });
            }
            viewModel.SubTema = db.SubTemas.Find(viewModel.SubTema.SubTemaId);
            viewModel.Tema = db.Temas.Find(viewModel.SubTema.TemaId);
            return View(viewModel);
        }
        

        private void ActualizarCuerpo(Gramatica gramatica)
        {   
            gramatica.GramaticaCuerpo.Cuerpo = FileUploadHelper.ActualizarImagenesCuerpo(gramatica.GramaticaCuerpo.Cuerpo,gramatica.GramaticaId, "media/Gramatica/");
        
            db.Entry(gramatica.GramaticaCuerpo).State = EntityState.Modified;
            db.SaveChanges();
        }

        // GET: Admin/Gramaticas/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gramatica gramatica = db.Gramaticas.Include(gra=> gra.GramaticaCuerpo).FirstOrDefault(a => a.GramaticaId == id);
            if (gramatica == null)
            {
                return HttpNotFound();
            }


            GramaticasEditViewModel viewModel = new GramaticasEditViewModel();

            viewModel.Gramatica = gramatica;
            viewModel.SubTema = db.SubTemas.Find(gramatica.SubTemaId);
            viewModel.Tema = db.Temas.Find(viewModel.SubTema.TemaId);
            viewModel.CargarClienteSeleccionado(db);
            return View(viewModel);
        }

        // POST: Admin/Gramaticas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(GramaticasEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                ActualizarCuerpo(viewModel.Gramatica);
                viewModel.Gramatica.Cuerpo = null;
                db.Entry(viewModel.Gramatica).State = EntityState.Modified;
                db.SaveChanges();
                AccesoClientesHelper.AnyadirGramaticaConHijos(viewModel.Gramatica.GramaticaId, viewModel.Clientes.Where(cli => cli.Selected)
                    .Select(cli => Convert.ToInt32(cli.Value)).ToList());

                return RedirectToAction("Index", "Bloques", new { id = viewModel.SubTema.SubTemaId, pestanya = (int)PestanyasBloques.Gramaticas });
            }

            viewModel.SubTema = db.SubTemas.Find(viewModel.SubTema.SubTemaId);
            viewModel.Tema = db.Temas.Find(viewModel.SubTema.TemaId);

            return View(viewModel);
        }

        // GET: Admin/Gramaticas/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gramatica gramatica = db.Gramaticas.Find(id);
            if (gramatica == null)
            {
                return HttpNotFound();
            }

            //Ordenar de nuevo todos los Gramaticas            
            foreach (Gramatica gr in db.Gramaticas.Where(b => b.SubTemaId == gramatica.SubTemaId && b.Orden > gramatica.Orden).ToList())
            {
                gr.Orden = (gr.Orden) - 1;

                db.Entry(gr).State = EntityState.Modified;
            }
            db.SaveChanges();
                            
            //Eliminar subtema
            db.Gramaticas.Remove(gramatica);
            db.SaveChanges();
            return RedirectToAction("Index", "Bloques", new { id = gramatica.SubTemaId, pestanya = (int)PestanyasBloques.Gramaticas });
        }

        // GET: Admin/Gramaticas/Move
        public ActionResult Move(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gramatica gramatica = db.Gramaticas.Find(id);
            if (gramatica == null)
            {
                return HttpNotFound();
            }
            var subtema = db.SubTemas.Include(s => s.Tema).SingleOrDefault(s => s.SubTemaId == gramatica.SubTemaId);

            GramaticasMoveViewModel viewModel = new GramaticasMoveViewModel();
            viewModel.Gramatica = gramatica;
            viewModel.SubTema = subtema;
            viewModel.InicializarDesplegables();

            return View(viewModel);
        }

        // POST: Admin/Gramaticas/Move
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Move(GramaticasMoveViewModel viewModel)
        {
            int id = viewModel.Gramatica.GramaticaId;
            Gramatica gramatica = db.Gramaticas.Find(id);
            var subtema = db.SubTemas.Include(s => s.Tema).SingleOrDefault(s => s.SubTemaId == gramatica.SubTemaId);
            viewModel.SubTema = subtema;
            viewModel.Gramatica = gramatica;
            if (gramatica != null)
            {


                //Ordenar de nuevo todas los Gramaticas            
                foreach (Gramatica gr in db.Gramaticas.Where(b => b.SubTemaId == gramatica.SubTemaId && b.Orden > gramatica.Orden).ToList())
                {
                    gr.Orden = (gr.Orden) - 1;

                    db.Entry(gr).State = EntityState.Modified;
                }
                db.SaveChanges();

                viewModel.Gramatica.SubTemaId = viewModel.SubTemaCopiarId;
                viewModel.Gramatica.Orden = db.Gramaticas.Count(grama => grama.SubTemaId == viewModel.SubTemaCopiarId) + 1;
                db.Entry(viewModel.Gramatica).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index", "Bloques", new { id = viewModel.Gramatica.SubTemaId, pestanya = (int)PestanyasBloques.Gramaticas });
            }

            viewModel.InicializarDesplegables();
            return View(viewModel);
        }

        // GET: Admin/Gramaticas/Copy
        public ActionResult Copy(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gramatica gramatica = db.Gramaticas.Find(id);
            if (gramatica == null)
            {
                return HttpNotFound();
            }
            var subtema = db.SubTemas.Include(s => s.Tema).SingleOrDefault(s => s.SubTemaId == gramatica.SubTemaId);

            GramaticasMoveViewModel viewModel = new GramaticasMoveViewModel();
            viewModel.Gramatica = gramatica;
            viewModel.SubTema = subtema;
            viewModel.InicializarDesplegables();

            return View(viewModel);
        }

        // POST: Admin/Gramaticas/Copy
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Copy(GramaticasMoveViewModel viewModel)
        {
            int id = viewModel.Gramatica.GramaticaId;
            Gramatica gramatica = db.Gramaticas.Include(gra => gra.GramaticaCuerpo).Include(g => g.Vocabularios).FirstOrDefault(g => g.GramaticaId == id);

            var subtemaOrigen = db.SubTemas.Include(s => s.Tema).SingleOrDefault(s => s.SubTemaId == gramatica.SubTemaId);

            if (gramatica != null)
            {
                //copia de gramatica y vocabularios
                Gramatica gramaticaNuevo = new Gramatica();
                gramaticaNuevo = gramatica;
                gramaticaNuevo.SubTemaId = viewModel.SubTemaCopiarId;

                //orden
                int nuevoOrden = db.Gramaticas.Where(g => g.SubTemaId == viewModel.SubTemaCopiarId).Count();
                gramaticaNuevo.Orden = nuevoOrden + 1;

                db.Gramaticas.Add(gramaticaNuevo);
                db.SaveChanges();
                

                return RedirectToAction("Index", "Bloques", new { id = gramaticaNuevo.SubTemaId, pestanya = (int)PestanyasBloques.Gramaticas });
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
            Gramatica gramatica = db.Gramaticas.Find(id);
            if (gramatica == null)
            {
                return HttpNotFound();
            }

            actualizarOrdenSubtema(id, 1, posicion);
            

            return RedirectToAction("Index","Bloques", new { id = gramatica.SubTemaId });
        }
        

        private void actualizarOrdenSubtema(int id, int posicion, string mov)
        {
            Gramatica gramatica = db.Gramaticas.Find(id);
            Int32 ordenNuevo;

            if (mov.Equals("top"))
            {
                //Al subtema seleccionado le restamos 1 posicion
                ordenNuevo = (gramatica.Orden) - 1;

                //Buscamos el subtema con el orden a modificar para sumarle 1 posicion
                Gramatica gr = db.Gramaticas.FirstOrDefault(b=>b.SubTemaId==gramatica.SubTemaId && b.Orden == ordenNuevo);
                gr.Orden = (gr.Orden) + 1;
                db.Entry(gr).State = EntityState.Modified;
                db.SaveChanges();

                gramatica.Orden = ordenNuevo;
                db.Entry(gramatica).State = EntityState.Modified;
                db.SaveChanges();   
            }

            if (mov.Equals("down"))
            {
                //Al subtema seleccionado le sumamos 1 posicion
                ordenNuevo = (gramatica.Orden) + 1;

                //Buscamos el subtema con el orden a modificar para restarle 1 posicion
                Gramatica gr = db.Gramaticas.FirstOrDefault(b => b.SubTemaId == gramatica.SubTemaId && b.Orden == ordenNuevo);
                gr.Orden = (gr.Orden) - 1;
                db.Entry(gr).State = EntityState.Modified;
                db.SaveChanges();

                gramatica.Orden = ordenNuevo;
                db.Entry(gramatica).State = EntityState.Modified;
                db.SaveChanges();
            }
            ControlarHuecosOrdenGramatica(gramatica);
        }

        private void ControlarHuecosOrdenGramatica(Gramatica gramaticaOriginal)
        {
            List<Gramatica> gramaticas = db.Gramaticas.Where(gr => gr.SubTemaId == gramaticaOriginal.SubTemaId).OrderBy(gr => gr.Orden).ToList();
            int ordenEsperado = 1;
            bool ordenCorrecto = true;
            foreach(var gramatica in gramaticas)
            {
                if(ordenEsperado!= gramatica.Orden)
                {
                    ordenCorrecto = false;
                    gramatica.Orden = ordenEsperado;
                    db.Entry(gramatica).State = EntityState.Modified;

                }
                ordenEsperado++;
            }
            if (!ordenCorrecto)
            {
                db.SaveChanges();
            }
        }
    }
}
