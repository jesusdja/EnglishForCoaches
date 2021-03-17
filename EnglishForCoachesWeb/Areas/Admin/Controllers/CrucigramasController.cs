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
    public class CrucigramasController : MVCBaseController
    {
        private AuthContext db = new AuthContext();

        // GET: Admin/Crucigramas/Create
        public ActionResult Create(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bloque bloque = db.Bloques.SingleOrDefault(bl => bl.BloqueId == id);
            if (bloque == null)
            {
                return HttpNotFound();
            }
            CrucigramaCreateViewModel viewModel = new CrucigramaCreateViewModel();
            viewModel.Inicializar(id);
            viewModel.Crucigrama = db.Crucigramas.Include(c=>c.CasillaCrucigramas).FirstOrDefault(c => c.BloqueId == id);

            if(!viewModel.Crucigrama.CasillaCrucigramas.Any())
            {
                viewModel.Crucigrama = CrearCeldas(viewModel.Crucigrama);
            }

            viewModel.Letras = new string[12][];
            for (int i = 0; i < 12; i++)
            {
                viewModel.Letras[i] = new string[12];
                for (int j = 0; j < 12; j++)
                {
                    var casilla = viewModel.Crucigrama.CasillaCrucigramas.FirstOrDefault(cr => cr.PosH == j && cr.PosV == i);
                    if (casilla != null)
                    {

                        viewModel.Letras[i][j] = casilla.letra;
                    }
                }
            }
            return View(viewModel);
        }


        private Crucigrama CrearCeldas(Crucigrama crucigrama)
        {
            List<CasillaCrucigrama> icollection = new List<CasillaCrucigrama>();
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    icollection.Add(new CasillaCrucigrama()
                    {
                        letra = "",
                        PosH = j,
                        PosV = i
                    });
                }
            }
            crucigrama.CasillaCrucigramas = icollection;
            db.Crucigramas.Add(crucigrama);
            db.SaveChanges();
            return crucigrama;
        }

        // POST: Admin/Crucigramas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CrucigramaCreateViewModel viewModel)
        {           
            if (ModelState.IsValid)
            {
                viewModel.Crucigrama.Descripcion = viewModel.Crucigrama.Enunciado;
                
                db.Entry(viewModel.Crucigrama).State = EntityState.Modified;
                db.SaveChanges();

                viewModel.Crucigrama = db.Crucigramas.Include(c => c.CasillaCrucigramas).FirstOrDefault(c => c.BloqueId == viewModel.Crucigrama.BloqueId);
                for (int i = 0; i < 12; i++)
                {
                    for (int j = 0; j < 12; j++)
                    {
                        var casilla = viewModel.Crucigrama.CasillaCrucigramas.FirstOrDefault(cr => cr.PosH == j && cr.PosV == i);
                        casilla.letra = viewModel.Letras[i][j];
                        db.Entry(casilla).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                return RedirectToAction("Index", "Bloques", new { id = viewModel.Crucigrama.SubTemaId, pestanya = (int)PestanyasBloques.Contenidos });
            }
            viewModel.Inicializar(viewModel.Crucigrama.SubTemaId);
            return View(viewModel);
        }


        // GET: Admin/Crucigramas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Crucigrama Crucigrama = db.Crucigramas.Find(id);
            if (Crucigrama == null)
            {
                return HttpNotFound();
            }

            db.Crucigramas.Remove(Crucigrama);
            db.SaveChanges();
            return RedirectToAction("Create", "Crucigramas", new { id = Crucigrama.BloqueId });
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
