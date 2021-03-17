using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EnglishForCoachesWeb.Database;
using EnglishForCoachesWeb.Areas.Admin.Models;
using EnglishForCoachesWeb.Helpers;
using System.Text.RegularExpressions;
using EnglishForCoachesWeb.Controllers;

namespace EnglishForCoachesWeb.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ArticulosController : MVCBaseController
    {
        private AuthContext db = new AuthContext();

        // GET: Admin/Articulos
        public ActionResult Index()
        {
            var articulos = db.Articulos.Include(a => a.Categoria).OrderByDescending(art=>art.FechaPublicacion);
            return View(articulos.ToList());
        }

        // GET: Admin/Articulos/Create
        public ActionResult Create()
        {
            ArticuloCreateViewModel viewModel = new ArticuloCreateViewModel();
            viewModel.Inicializar();
            
            return View(viewModel);
        }

      
        // POST: Admin/Articulos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ImagenPreguntaFile,Tags,Articulo,"+
            "Articulo.ArticuloId,Articulo.CategoriaId,Articulo.FechaAlta,Articulo.Publicado,Articulo.Cuerpo"+
            "Articulo.FechaPublicacion,Articulo.Titulo,Articulo.ImagenDestacada")] ArticuloCreateViewModel model,
            [Bind(Include = "command")] string command)
        {

            if (command.Equals("Guardar y publicar"))
            {
                model.Articulo.Publicado = true;
            }
            else
            {
                model.Articulo.Publicado = false;
            }

            if (ModelState.IsValid)
            {
                model.Articulo.FechaAlta = DateTime.Now;

                model = (ArticuloCreateViewModel)ProcesarArticulo(model);

                db.Articulos.Add(model.Articulo);
                db.SaveChanges();

                SubirImagenes(model);
                ActualizarCuerpo(model);

                if (command.Equals("Guardar y publicar"))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Edit",new { id= model.Articulo .ArticuloId});
                }
            }

            model.InicializarDesplegables();
            return View(model);
        }

        private void SubirImagenes(ArticuloViewModel model)
        {
            if (model.ImagenPreguntaFile != null)
            {
                FileUploadHelper.SubirImagenArticulo(model.ImagenPreguntaFile, 1200, 370, Server.MapPath(Url.Content("~/Imagenes/")) + model.Articulo.ArticuloId + "_xl.jpg");
                FileUploadHelper.SubirImagenArticulo(model.ImagenPreguntaFile, 800, 640, Server.MapPath(Url.Content("~/Imagenes/")) + model.Articulo.ArticuloId + "_l.jpg");
                FileUploadHelper.SubirImagenArticulo(model.ImagenPreguntaFile, 370, 320, Server.MapPath(Url.Content("~/Imagenes/")) + model.Articulo.ArticuloId + "_m.jpg");
                FileUploadHelper.SubirImagenArticulo(model.ImagenPreguntaFile, 100, 76, Server.MapPath(Url.Content("~/Imagenes/")) + model.Articulo.ArticuloId + "_s.jpg");
            }
        }

        // GET: Admin/Articulos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Articulo articulo = db.Articulos.Include(a=>a.Categoria).Include(a => a.Tags).FirstOrDefault(ar=>ar.ArticuloId==id);
            if (articulo == null)
            {
                return HttpNotFound();
            }

            ArticuloEditViewModel viewModel = new ArticuloEditViewModel();
            viewModel.Inicializar(articulo);
            return View(viewModel);
        }

        // POST: Admin/Articulos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ImagenPreguntaFile,Tags,Articulo,"+
            "Articulo.ArticuloId,Articulo.CategoriaId,Articulo.FechaAlta,Articulo.Publicado,Articulo.Cuerpo"+
            "Articulo.FechaPublicacion,Articulo.Titulo,Articulo.ImagenDestacada")] ArticuloEditViewModel model,
            [Bind(Include = "command")] string command)
        {
            if (command.Equals("Guardar y publicar"))
            {
                model.Articulo.Publicado = true;
            }
            else
            {
                model.Articulo.Publicado = false;
            }

            if (ModelState.IsValid)
            {
                db.Articulos.Attach(model.Articulo);
                model.Articulo.Tags = db.Articulos.Include(Articulo=>Articulo.Tags).FirstOrDefault(art=>art.ArticuloId==model.Articulo.ArticuloId).Tags;

                model =(ArticuloEditViewModel)ProcesarArticulo(model);
                
                db.SaveChanges();

                SubirImagenes(model);
                ActualizarCuerpo(model);

                if (command.Equals("Guardar y publicar"))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Edit", new { id = model.Articulo.ArticuloId });
                }
            }

            model.InicializarDesplegables();
            return View(model);
         
        }

        private void ActualizarCuerpo(ArticuloViewModel model)
        {   
            model.Articulo.Cuerpo = FileUploadHelper.ActualizarImagenesCuerpo(model.Articulo.Cuerpo, model.Articulo.ArticuloId, "Imagenes/");

            model.Articulo.CuerpoResumen = HtmlTextHelper.QuitarEtiquetas(model.Articulo.Cuerpo);
            
            if (model.Articulo.CuerpoResumen.Length > 100)
            {
                model.Articulo.CuerpoResumen = model.Articulo.CuerpoResumen.Substring(0, 100);
            }
            db.Entry(model.Articulo).State = EntityState.Modified;
            db.SaveChanges();
        }

        private ArticuloViewModel ProcesarArticulo(ArticuloViewModel model)
        {
            model.Articulo.TituloSeo = FriendlyURL.SeekUrl(model.Articulo.Titulo, name => !db.Articulos.Any(t => t.TituloSeo == name && t.ArticuloId!=model.Articulo.ArticuloId));

            var tags = new List<string>();
            if (!string.IsNullOrWhiteSpace(model.Tags))
            {
                tags = model.Tags.ToLower().Split(',').ToList();
            }
            var listaTags = new List<Tag>();
            if (model.Articulo.Tags == null)
            {                
                model.Articulo.Tags = new List<Tag>();
            }

            foreach (var tag in tags)
            {
                var existeTag = db.Tags.FirstOrDefault(t => t.Descripcion == tag);
                if (existeTag == null)
                {
                    Tag nuevoTag = new Tag()
                    {
                        Descripcion = tag,
                        DescripcionSeo = FriendlyURL.SeekUrl(tag, name => !db.Tags.Any(t => t.DescripcionSeo == name))
                    };
                    db.Tags.Add(nuevoTag);
                    db.SaveChanges();
                }
            }

            // Remove deselected skills
            model.Articulo.Tags.Where(m => !tags.Contains(m.Descripcion))
            .ToList().ForEach(tag => model.Articulo.Tags.Remove(tag));

        // Add new skills
        var existingSkillIds = model.Articulo.Tags.Select(m => m.Descripcion);
        db.Tags.Where(m => tags.Except(existingSkillIds).Contains(m.Descripcion))
            .ToList().ForEach(tag => model.Articulo.Tags.Add(tag));

            

            return model;
        }


        // GET: Admin/Articulos/Delete/0
        public ActionResult Delete(int id)
        {
            Articulo articulo = db.Articulos.Find(id);
            db.Articulos.Remove(articulo);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Admin/Articulos/activar/0
        public ActionResult Publicar(int id)
        {
            var articulo = db.Articulos.Find(id);
            articulo.Publicado = !articulo.Publicado;
            db.Entry(articulo).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
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
