using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using EnglishForCoachesWeb.Database;
using EnglishForCoachesWeb.Database.Ejercicios;
using EnglishForCoachesWeb.Areas.Alumno.Models;
using EnglishForCoachesWeb.Controllers;
using EnglishForCoachesWeb.Helpers;
using EnglishForCoachesWeb.Auth;
using System.Collections.Generic;
using EnglishForCoachesWeb.Database.Foro;
using System;
using System.Security.Claims;
using EnglishForCoachesWeb.Database.Clientes;

namespace EnglishForCoachesWeb.Areas.Alumno.Controllers
{
    [Authorize(Roles = "Alumno,Admin")]
    public class ForoController : MVCBaseController
    {
        private AuthContext db = new AuthContext();


        // mark this action as unavailable to the general public
        [ChildActionOnly]
        public string NoLeidos()
        {
            var userId = ((ClaimsIdentity)User.Identity).FindFirst("UserId").Value;

            var numeroHilos = ForoHiloDataAccess.ObtenerForoHilo().Count();
            var idsHilos = ForoHiloDataAccess.ObtenerForoHilo().Select(hilo=>hilo.ForoHiloId).ToList();

            var numeroHilosLeidos = db.ForoHiloLeidos.Count(fhl=>fhl.AlumnoId==userId && idsHilos.Contains(fhl.ForoHiloId));
            int total = numeroHilos - numeroHilosLeidos;
            return total.ToString();
        }

        // GET: Alumno/Foro
        public ActionResult Index(int? categoriaId)
        {

            ForoIndexViewModel viewModel = new ForoIndexViewModel();
            var hilos = new List<ForoHilo>();
            
                viewModel.categoriasLateral = db.ForoCategorias.ToList();

            if (categoriaId.HasValue)
            {
                viewModel.categorias = db.ForoCategorias.Where(cat => cat.ForoCategoriaId == categoriaId.Value).ToList();

                hilos = ForoHiloDataAccess.ObtenerForoHilo().Where(fh => fh.ForoCategoriaId == categoriaId.Value).Take(10).ToList();
            }
            else
            {
                viewModel.categorias = db.ForoCategorias.ToList();
                foreach (var categoria in viewModel.categorias)
                {
                    hilos.AddRange(ForoHiloDataAccess.ObtenerForoHilo().Where(fh => fh.ForoCategoriaId == categoria.ForoCategoriaId).Take(3));
                }
            }


            viewModel.hilos = hilos;

            return View(viewModel);
        }

        // POST: Alumno/Vocabularios
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(ForoIndexViewModel viewModel)
        {
            viewModel.categorias = db.ForoCategorias.ToList();
            viewModel.categoriasLateral = db.ForoCategorias.ToList();

            var hilos = new List<ForoHilo>();
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(viewModel.TextoBusqueda))
                {
                    hilos = ForoHiloDataAccess.ObtenerForoHilo().Where(fh => fh.Titulo.ToUpper().Contains(viewModel.TextoBusqueda.ToUpper())).ToList();
                    var idsCategorias = hilos.Select(hil => hil.ForoCategoriaId).ToList();
                    viewModel.categorias = viewModel.categorias.Where(fc => idsCategorias.Contains(fc.ForoCategoriaId)).ToList();
                }
                else
                {
                    foreach (var categoria in viewModel.categorias)
                    {
                        hilos.AddRange(ForoHiloDataAccess.ObtenerForoHilo().Where(fh => fh.ForoCategoriaId == categoria.ForoCategoriaId).Take(3));
                    }
                }
            }
            else
            {
                foreach (var categoria in viewModel.categorias)
                {
                    hilos.AddRange(ForoHiloDataAccess.ObtenerForoHilo().Where(fh => fh.ForoCategoriaId == categoria.ForoCategoriaId).Take(3));
                }
            }

            viewModel.hilos = hilos;

            return View(viewModel);
        }


        // GET: Alumno/Foro/Create
        public ActionResult Create(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var categoria = db.ForoCategorias.SingleOrDefault(s => s.ForoCategoriaId == id);
            if (categoria == null)
            {
                return HttpNotFound();
            }


            ForoCreateViewModel viewModel = new ForoCreateViewModel();
            viewModel.ForoCategoria = categoria;

            return View(viewModel);
        }


        // POST:Alumno/Foro/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ForoCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var userId = ((ClaimsIdentity)User.Identity).FindFirst("UserId").Value;
                viewModel.hilo.AlumnoId = userId;
                viewModel.hilo.AlumnoRespuestaId = userId;
                var NombreUsuario = ((ClaimsIdentity)User.Identity).FindFirst("NombreUsuario").Value;
                var ClienteId = ((ClaimsIdentity)User.Identity).FindFirst("ClienteId").Value;
                if (string.IsNullOrEmpty(ClienteId))
                {
                    ClienteId = ((int)ClientesId.Azulita).ToString();
                }

                viewModel.hilo.CreadoPor = NombreUsuario;
                viewModel.hilo.RespondidoPor = NombreUsuario;
                viewModel.hilo.FechaCreacion = DateTime.Now;
                viewModel.hilo.FechaRespuesta = DateTime.Now;
                viewModel.hilo.ForoCategoriaId = viewModel.ForoCategoria.ForoCategoriaId;
                if (User.IsInRole("Admin"))
                {
                    string cliente = (string)RouteData.Values["cliente"];
                    var clienteActual = db.Clientes.FirstOrDefault(cl => cl.NombreUrl.ToUpper() == cliente.ToUpper());
                    if (clienteActual == null)
                    {
                        clienteActual = db.Clientes.Find((int)ClientesId.Azulita);
                    }
                    ClienteId = clienteActual.ClienteId.ToString();

                    viewModel.hilo.RespondidoPorAdmin = true;
                    viewModel.hilo.Admin =true;
                }else
                {
                    viewModel.hilo.RespondidoPorAdmin = false;
                    viewModel.hilo.Admin = false;
                }

                viewModel.hilo.ClienteId = Convert.ToInt32(ClienteId);

                db.ForoHilos.Add(viewModel.hilo);
                db.SaveChanges();

                return RedirectToAction("Hilo", "Foro", new { id = viewModel.hilo.ForoHiloId });
            }

            var categoria = db.ForoCategorias.SingleOrDefault(s => s.ForoCategoriaId == viewModel.ForoCategoria.ForoCategoriaId);
            viewModel.ForoCategoria = categoria;
            return View(viewModel);
        }

        // GET: Alumno/Foro/Hilo
        public ActionResult Hilo(int id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var hilo = db.ForoHilos.Include(bl => bl.ForoCategoria).Include(bl => bl.Mensajes).FirstOrDefault(b => b.ForoHiloId == id);

            if (hilo == null)
                return HttpNotFound();

            var userId = ((ClaimsIdentity)User.Identity).FindFirst("UserId").Value;

            var existe = db.ForoHiloLeidos.FirstOrDefault(fhl => fhl.AlumnoId == userId && fhl.ForoHiloId == id);
            if (existe == null)
            {
                ForoHiloLeido foroLeido = new ForoHiloLeido()
                {
                    AlumnoId = userId,
                    ForoHiloId = id
                };

                db.ForoHiloLeidos.Add(foroLeido);
                db.SaveChanges();
            }

            ForoHiloViewModel viewModel = new ForoHiloViewModel();
            viewModel.hilo = hilo;
            return View(viewModel);
        }

        // POST:Alumno/Foro/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Hilo(ForoHiloViewModel viewModel, int id)
        {
            var hilo = db.ForoHilos.Include(bl => bl.ForoCategoria).Include(bl => bl.Mensajes).FirstOrDefault(b => b.ForoHiloId == id);
            viewModel.hilo = hilo;

            if (!string.IsNullOrEmpty(viewModel.ForoMensaje.Texto))
            {
                var userId = ((ClaimsIdentity)User.Identity).FindFirst("UserId").Value;
                var NombreUsuario = ((ClaimsIdentity)User.Identity).FindFirst("NombreUsuario").Value;

                viewModel.hilo.AlumnoRespuestaId = userId;
                viewModel.hilo.RespondidoPor = NombreUsuario;
                viewModel.hilo.FechaRespuesta = DateTime.Now;
                if (User.IsInRole("Admin"))
                {
                    viewModel.hilo.RespondidoPorAdmin = true;
                }
                else
                {
                    viewModel.hilo.RespondidoPorAdmin = false;
                }

                db.Entry(viewModel.hilo).State = EntityState.Modified;

                viewModel.ForoMensaje.ForoHiloId = viewModel.hilo.ForoHiloId;
                viewModel.ForoMensaje.CreadoPor = NombreUsuario;
                viewModel.ForoMensaje.AlumnoId = userId;
                viewModel.ForoMensaje.FechaCreacion = DateTime.Now;


                if (User.IsInRole("Admin"))
                {
                    viewModel.ForoMensaje.Admin = true;
                }
                else
                {
                    viewModel.ForoMensaje.Admin = false;
                }

                db.ForoMensajes.Add(viewModel.ForoMensaje);
                db.SaveChanges();

                var leidos = db.ForoHiloLeidos.Where(fhl => fhl.ForoHiloId == viewModel.hilo.ForoHiloId).ToList();

                db.ForoHiloLeidos.RemoveRange(leidos);
                db.SaveChanges();

                return RedirectToAction("Hilo", "Foro", new { id = viewModel.hilo.ForoHiloId });
            }                   
            
                
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

