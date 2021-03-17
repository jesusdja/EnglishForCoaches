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
using System.Security.Claims;

namespace EnglishForCoachesWeb.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,AdministradorGrupo")]
    public class NoticiasController : MVCBaseController
    {
        private AuthContext db = new AuthContext();


        // GET: Admin/Noticias
        public ActionResult Index()
        {
            NoticiaIndexViewModel viewModel = new NoticiaIndexViewModel();
            var busqueda = db.Noticias.Include(Not => Not.NoticiaGrupos.Select(gr => gr.GrupoUsuario));
            if (User.IsInRole("AdministradorGrupo"))
            {
                var ClienteId = Convert.ToInt32(((ClaimsIdentity)User.Identity).FindFirst("ClienteId").Value);
                var GrupoUsuarioId = Convert.ToInt32(((ClaimsIdentity)User.Identity).FindFirst("GrupoUsuario").Value);
                busqueda = busqueda.Where(not => not.ClienteId== ClienteId && not.NoticiaGrupos.Select(y => y.GrupoUsuarioId).Contains(GrupoUsuarioId));
            }

            viewModel.Pagina = 1;

            viewModel.CalcularPaginacion( busqueda.Count());
            viewModel.listadoNoticias = busqueda.OrderByDescending(not => not.NoticiaId).Take(viewModel.resultadosPorPagina).ToList();
            return View(viewModel);
        }



        // POST: Admin/Noticias/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(NoticiaIndexViewModel viewModel)
        {
            var busqueda = db.Noticias.Include(Not => Not.NoticiaGrupos.Select(gr => gr.GrupoUsuario));
            if (User.IsInRole("AdministradorGrupo"))
            {
                var ClienteId = Convert.ToInt32(((ClaimsIdentity)User.Identity).FindFirst("ClienteId").Value);
                var GrupoUsuarioId = Convert.ToInt32(((ClaimsIdentity)User.Identity).FindFirst("GrupoUsuario").Value);
                busqueda = busqueda.Where(not => not.ClienteId == ClienteId && not.NoticiaGrupos.Select(y => y.GrupoUsuarioId).Contains(GrupoUsuarioId));
            }


            if (!string.IsNullOrWhiteSpace(viewModel.TextoBusqueda))
            {
                busqueda = busqueda.Where(x => x.Titulo.Contains(viewModel.TextoBusqueda));
            }

            viewModel.CalcularPaginacion( busqueda.Count());

            int skip = (viewModel.Pagina - 1) * viewModel.resultadosPorPagina;
            viewModel.listadoNoticias = busqueda.OrderByDescending(not => not.NoticiaId).Skip(skip).Take(viewModel.resultadosPorPagina).ToList();
            return View(viewModel);
        }

        // GET: Admin/Noticias/Create
        public ActionResult Create( )
        {            NoticiaCreateViewModel viewModel = new NoticiaCreateViewModel();

            viewModel.InicializarDesplegables();
            return View(viewModel);
        }

        // POST: Admin/Noticias/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NoticiaCreateViewModel viewModel)
        {
            //fichero pdf
            //"~/media/upload/Noticias_puntos/"

            if (ModelState.IsValid)
            {

                var UserId = ((ClaimsIdentity)User.Identity).FindFirst("UserId").Value;
                viewModel.Noticia.UsuarioId = UserId;
                viewModel.Noticia.Fecha = DateTime.Now;
                viewModel.Noticia.TextoResumen = HtmlTextHelper.QuitarEtiquetas(viewModel.Noticia.Texto);
                if (viewModel.Noticia.TextoResumen.Length > 200)
                {
                    viewModel.Noticia.TextoResumen = viewModel.Noticia.TextoResumen.Substring(0, 200);
                }
                if (User.IsInRole("AdministradorGrupo"))
                {
                    var ClienteId = ((ClaimsIdentity)User.Identity).FindFirst("ClienteId").Value;
                    viewModel.Noticia.ClienteId = Convert.ToInt32(ClienteId);
                }
                    db.Noticias.Add(viewModel.Noticia);
                db.SaveChanges();
                if (viewModel.File != null)
                {
                    viewModel.Noticia.FicheroAdjunto = viewModel.Noticia.NoticiaId +"_"+ viewModel.File.FileName;

                    string nameAndLocation = "~/media/upload/Noticias_adjuntos/" + viewModel.Noticia.FicheroAdjunto;
                    viewModel.File.SaveAs(Server.MapPath(nameAndLocation));

                    db.Entry(viewModel.Noticia).State = EntityState.Modified;
                    db.SaveChanges();
                }
                if (viewModel.ImageFile != null)
                {
                    viewModel.Noticia.Foto = viewModel.Noticia.NoticiaId + ".jpg";

                    string nameAndLocation = "~/media/upload/Noticias_imagenes/" + viewModel.Noticia.Foto;
                  
                     FileUploadHelper.SubirImagenArticulo(viewModel.ImageFile, 800, 640, Server.MapPath(Url.Content(nameAndLocation)));

                    db.Entry(viewModel.Noticia).State = EntityState.Modified;
                    db.SaveChanges();
                }
                
                if (User.IsInRole("AdministradorGrupo"))
                {
                    var GrupoUsuarioId = ((ClaimsIdentity)User.Identity).FindFirst("GrupoUsuario").Value;

                    NoticiaGrupo noticiaGrupo = new NoticiaGrupo()
                    {
                        GrupoUsuarioId = Convert.ToInt32(GrupoUsuarioId),
                        NoticiaId = viewModel.Noticia.NoticiaId
                    };
                    db.NoticiaGrupos.Add(noticiaGrupo);
                    db.SaveChanges();

                }
                if (User.IsInRole("Admin"))
                {
                    foreach (var grupo in viewModel.GruposUsuarios)
                    {
                        if (grupo.Seleccionado)
                        {
                            NoticiaGrupo noticiaGrupo = new NoticiaGrupo()
                            {
                                GrupoUsuarioId = grupo.Id,
                                NoticiaId = viewModel.Noticia.NoticiaId
                            };
                            db.NoticiaGrupos.Add(noticiaGrupo);
                            db.SaveChanges();
                        }
                    }
                }

                return RedirectToAction("Index",new { });
            }
            viewModel.InicializarDesplegables();
            return View(viewModel);
        }

        // GET: Admin/Noticias/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Noticia noticia = db.Noticias.Include(not => not.NoticiaGrupos).FirstOrDefault(a => a.NoticiaId == id);
            if (noticia == null)
            {
                return HttpNotFound();
            }


            NoticiaEditViewModel viewModel = new NoticiaEditViewModel();

            viewModel.InicializarDesplegables();
            viewModel.Noticia = noticia;
            foreach (var grupo in viewModel.GruposUsuarios)
            {
                if (noticia.NoticiaGrupos.FirstOrDefault(gr => gr.GrupoUsuarioId == grupo.Id) != null)
                {
                    grupo.Seleccionado = true;
                }
            }

            return View(viewModel);
        }

        // POST: Admin/Noticias/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(NoticiaEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {

                if (viewModel.File != null)
                {
                    string fullPath = Request.MapPath("~/media/upload/Noticias_adjuntos/" + viewModel.Noticia.FicheroAdjunto);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                    viewModel.Noticia.FicheroAdjunto = viewModel.Noticia.NoticiaId + "_" + viewModel.File.FileName;

                    string nameAndLocation = "~/media/upload/Noticias_audios/" + viewModel.Noticia.FicheroAdjunto;
                    viewModel.File.SaveAs(Server.MapPath(nameAndLocation));
                }

                if (viewModel.ImageFile != null)
                {
                    string fullPath = Request.MapPath("~/media/upload/Noticias_imagenes/" + viewModel.Noticia.Foto);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                    viewModel.Noticia.Foto = viewModel.Noticia.NoticiaId + ".jpg";

                    string nameAndLocation = "~/media/upload/Noticias_imagenes/" + viewModel.Noticia.Foto;
                    viewModel.ImageFile.SaveAs(Server.MapPath(nameAndLocation));
                }
                viewModel.Noticia.TextoResumen = HtmlTextHelper.QuitarEtiquetas(viewModel.Noticia.Texto);
                if(viewModel.Noticia.TextoResumen.Length>200)
                {
                    viewModel.Noticia.TextoResumen = viewModel.Noticia.TextoResumen.Substring(0,200);
                }

                db.Entry(viewModel.Noticia).State = EntityState.Modified;
                db.SaveChanges();

                foreach (var grupo in viewModel.GruposUsuarios)
                {
                    var existe = db.NoticiaGrupos.FirstOrDefault(gr=>gr.NoticiaId== viewModel.Noticia.NoticiaId && gr.GrupoUsuarioId==grupo.Id);

                    if (grupo.Seleccionado&& existe==null)
                    {
                        NoticiaGrupo noticiaGrupo = new NoticiaGrupo()
                        {
                            GrupoUsuarioId = grupo.Id,
                            NoticiaId = viewModel.Noticia.NoticiaId
                        };
                        db.NoticiaGrupos.Add(noticiaGrupo);
                        db.SaveChanges();
                    }
                    if (!grupo.Seleccionado && existe != null)
                    {
                        db.NoticiaGrupos.Remove(existe);
                        db.SaveChanges();
                    }
                }

                return RedirectToAction("Index", new {});
            }

            viewModel.InicializarDesplegables();
            return View(viewModel);
        }

        // GET: Admin/Noticias/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Noticia Noticia = db.Noticias.Find(id);
            if (Noticia == null)
            {
                return HttpNotFound();
            }
                 

            string fullPath = Request.MapPath("~/media/upload/Noticias_adjuntos/" + Noticia.FicheroAdjunto);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
            fullPath = Request.MapPath("~/media/upload/Noticias_imagenes/" + Noticia.Foto);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
            //Eliminar subtema
            db.Noticias.Remove(Noticia);
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
