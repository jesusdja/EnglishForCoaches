using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EnglishForCoachesWeb.Database;
using EnglishForCoachesWeb.Database.Clientes;
using EnglishForCoachesWeb.Areas.Admin.Models;
using EnglishForCoachesWeb.Controllers;
using System.Drawing;

namespace EnglishForCoachesWeb.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ClientesController : MVCBaseController
    {
        private AuthContext db = new AuthContext();
        private int minColor=127;

        // GET: Admin/Clientes
        public ActionResult Index()
        {
            ClienteIndexViewModel viewModel = new ClienteIndexViewModel();
            viewModel.Pagina = 1;
            var busqueda = db.Clientes.OrderBy(au => au.Abreviatura);
            viewModel.CalcularPaginacion( busqueda.Count());
            viewModel.listadoClientes = busqueda.Take(viewModel.resultadosPorPagina).ToList();
            return View(viewModel);
        }

    

        // POST: Admin/AccesoUsuarios/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(ClienteIndexViewModel viewModel)
        {
            var busqueda = db.Clientes.OrderBy(au => au.Abreviatura).ToList();

            if (!string.IsNullOrWhiteSpace(viewModel.TextoBusqueda))
            {
                busqueda = busqueda.Where(x => x.Abreviatura.ToLower().Contains(viewModel.TextoBusqueda.ToLower()) || x.Descripcion.ToLower().Contains(viewModel.TextoBusqueda.ToLower())).ToList();
            }

            viewModel.CalcularPaginacion( busqueda.Count());

            int skip = (viewModel.Pagina - 1) * viewModel.resultadosPorPagina;
            viewModel.listadoClientes = busqueda.Skip(skip).Take(viewModel.resultadosPorPagina).ToList();
            return View(viewModel);
        }

        // GET: Admin/Clientes/Create
        public ActionResult Create( )
        {            ClienteCreateViewModel viewModel = new ClienteCreateViewModel();
  

            return View(viewModel);
        }

        // POST: Admin/Clientes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ClienteCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {

                bool valido = true;
                int noValido = 0;
                Color color = (Color)new ColorConverter().ConvertFromString(viewModel.Cliente.EstiloColor);
                if (color.R <= minColor)
                {
                    noValido++;
                    ModelState.AddModelError("Cliente.EstiloColor", "El color es demasiado claro, debe ser más oscuro para poder mostrar texto blanco encima");
                }
                if (color.B <= minColor)
                {
                    noValido++;
                    ModelState.AddModelError("Cliente.EstiloColor", "El color es demasiado claro, debe ser más oscuro para poder mostrar texto blanco encima");
                }
                if (color.G <= minColor)
                {
                    noValido++;
                    ModelState.AddModelError("Cliente.EstiloColor", "El color es demasiado claro, debe ser más oscuro para poder mostrar texto blanco encima");
                }
                if(noValido==3)
                {
                    valido = false;
                }

                if (string.IsNullOrEmpty(viewModel.Cliente.NombreUrl))
                {
                    valido = false;
                    ModelState.AddModelError("Cliente.NombreUrl", "El nombreUrl no puede estar vacío.");
                }
                else
                {
                    if (db.Clientes.Any(cl=>cl.NombreUrl== viewModel.Cliente.NombreUrl))
                    {
                        valido = false;
                        ModelState.AddModelError("Cliente.NombreUrl", "Ya existe un cliente con este nombreUrl.");
                    }
                }
                if (valido)
                {
                    db.Clientes.Add(viewModel.Cliente);
                    db.SaveChanges();

                    if (viewModel.ImageFileBlanco != null)
                    {
                        viewModel.Cliente.LogoBlanco = viewModel.Cliente.ClienteId + "_logoBlanco.jpg";

                        string nameAndLocation = "~/media/upload/Cliente/" + viewModel.Cliente.LogoBlanco;
                        viewModel.ImageFileBlanco.SaveAs(Server.MapPath(nameAndLocation));

                        db.Entry(viewModel.Cliente).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    if (viewModel.ImageFileNegro != null)
                    {
                        viewModel.Cliente.LogoNegro = viewModel.Cliente.ClienteId + "_logoNegro.jpg";

                        string nameAndLocation = "~/media/upload/Cliente/" + viewModel.Cliente.LogoNegro;
                        viewModel.ImageFileNegro.SaveAs(Server.MapPath(nameAndLocation));

                        db.Entry(viewModel.Cliente).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    return RedirectToAction("Index", new { });
                }
            }
            return View(viewModel);
        }

        // GET: Admin/Clientes/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente Cliente = db.Clientes.FirstOrDefault(a => a.ClienteId == id);
            if (Cliente == null)
            {
                return HttpNotFound();
            }


            ClienteEditViewModel viewModel = new ClienteEditViewModel();
           

            viewModel.Cliente = Cliente;
            return View(viewModel);
        }

        // POST: Admin/Clientes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ClienteEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {



                bool valido = true;
                int noValido = 0;
                Color color = (Color)new ColorConverter().ConvertFromString(viewModel.Cliente.EstiloColor);
                if (color.R <= minColor)
                {
                    noValido++;
                    ModelState.AddModelError("Cliente.EstiloColor", "El color es demasiado claro, debe ser más oscuro para poder mostrar texto blanco encima");
                }
                if (color.B <= minColor)
                {
                    noValido++;
                    ModelState.AddModelError("Cliente.EstiloColor", "El color es demasiado claro, debe ser más oscuro para poder mostrar texto blanco encima");
                }
                if (color.G <= minColor)
                {
                    noValido++;
                    ModelState.AddModelError("Cliente.EstiloColor", "El color es demasiado claro, debe ser más oscuro para poder mostrar texto blanco encima");
                }
                if (noValido == 3)
                {
                    valido = false;
                }

                if (valido)
                {
                    db.Entry(viewModel.Cliente).State = EntityState.Modified;
                    db.SaveChanges();


                    if (viewModel.ImageFileBlanco != null)
                    {
                        if (viewModel.Cliente.LogoBlanco != null)
                        {
                            var fullPath = Request.MapPath("~/media/upload/Cliente/" + viewModel.Cliente.LogoBlanco);
                            if (System.IO.File.Exists(fullPath))
                            {
                                System.IO.File.Delete(fullPath);
                            }
                        }

                        viewModel.Cliente.LogoBlanco = viewModel.Cliente.ClienteId + "_logoBlanco.jpg";

                        string nameAndLocation = "~/media/upload/Cliente/" + viewModel.Cliente.LogoBlanco;
                        viewModel.ImageFileBlanco.SaveAs(Server.MapPath(nameAndLocation));
                        db.Entry(viewModel.Cliente).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    if (viewModel.ImageFileNegro != null)
                    {
                        if (viewModel.Cliente.LogoNegro != null)
                        {
                            var fullPath = Request.MapPath("~/media/upload/Cliente/" + viewModel.Cliente.LogoNegro);
                            if (System.IO.File.Exists(fullPath))
                            {
                                System.IO.File.Delete(fullPath);
                            }
                        }

                        viewModel.Cliente.LogoNegro = viewModel.Cliente.ClienteId + "_logoNegro.jpg";

                        string nameAndLocation = "~/media/upload/Cliente/" + viewModel.Cliente.LogoNegro;
                        viewModel.ImageFileNegro.SaveAs(Server.MapPath(nameAndLocation));
                        db.Entry(viewModel.Cliente).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    return RedirectToAction("Index", new { });
                }
            }

            return View(viewModel);
        }

        // GET: Admin/Clientes/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente Cliente = db.Clientes.Find(id);
            if (Cliente == null)
            {
                return HttpNotFound();
            }
            if (Cliente.LogoBlanco != null)
            {
                string fullPath = Request.MapPath("~/media/upload/Cliente/" + Cliente.LogoBlanco);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }
            if (Cliente.LogoNegro != null)
            {
                string fullPath = Request.MapPath("~/media/upload/Cliente/" + Cliente.LogoNegro);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }


            //Eliminar 
            db.Clientes.Remove(Cliente);
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
