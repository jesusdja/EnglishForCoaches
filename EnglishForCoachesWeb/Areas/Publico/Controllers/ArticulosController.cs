using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EnglishForCoachesWeb.Database;
using EnglishForCoachesWeb.Areas.Publico.Models;

namespace EnglishForCoachesWeb.Areas.Publico.Controllers
{
    public class ArticulosController : Controller
    {
        private AuthContext db = new AuthContext();
        int articulosPorPagina = 6;

        // GET: Publico/Articulos
        public ActionResult Index()
        {
            var fechaActual = DateTime.Now;

            IndexViewModel model = new IndexViewModel();

            model.ArticulosDestacados = db.Articulos.Include(a => a.Categoria).Where(art => art.Publicado && art.FechaPublicacion <= fechaActual)
                .OrderByDescending(art => art.FechaPublicacion).Take(3).ToList();
            model.ArticulosLista = db.Articulos.Include(a => a.Categoria).Where(art => art.Publicado && art.FechaPublicacion <= fechaActual)
                .OrderByDescending(art => art.FechaPublicacion).Skip(3).Take(8).ToList();
            return View(model);
        }

        // GET: Publico/Articulos/Articulo/5
        public ActionResult Articulo(string categoriaUrl,string articuloUrl)
        {
            var categoria = db.Categorias.FirstOrDefault(cat => cat.DescripcionSeo == categoriaUrl.ToLower());
            if (categoria == null)
            {
                return HttpNotFound();
            }
            Articulo articulo = db.Articulos.Include(a => a.Categoria).Include(a => a.Tags).Include(a => a.Comentarios).FirstOrDefault(art=>art.TituloSeo== articuloUrl.ToLower());
            if (articulo == null)
            {
                return HttpNotFound();
            }
            if(!articulo.Publicado)
            {
                return HttpNotFound();
            }
            if (articulo.FechaPublicacion>DateTime.Now)
            {
                return HttpNotFound();
            }
            var fechaActual = DateTime.Now;

            ArticuloDetailViewModel model = new ArticuloDetailViewModel();
            model.Articulo = articulo;
            model.ArticuloId = articulo.ArticuloId;
            model.ArticulosRecientes = db.Articulos.Include(a => a.Categoria).Where(art => art.Publicado && art.FechaPublicacion <= fechaActual)
             .OrderByDescending(art => art.FechaPublicacion).Take(6).ToList();

            return View(model);
        }

        // GET: Publico/Articulos/Categoria/1
        public ActionResult Categoria(string categoriaUrl,int? page)//,string articuloUrl)
        {
            var fechaActual = DateTime.Now;
            CategoriaViewModel model = new CategoriaViewModel();

            var categoria = db.Categorias.FirstOrDefault(cat=>cat.DescripcionSeo== categoriaUrl.ToLower());
            if (categoria == null)
            {
                return HttpNotFound();
            }
            var articulos = db.Articulos.Include(a => a.Categoria).Where(art => art.Publicado && art.FechaPublicacion <= fechaActual)
                .OrderByDescending(art => art.FechaPublicacion);
            var articulosFiltrados = articulos.Where(art => art.CategoriaId == categoria.CategoriaId);

            if (page.HasValue)
            {
                model.PaginaActual = page.Value;
            }
            else
            {
                model.PaginaActual = 1;
            }
            double numPag = articulosFiltrados.Count() / articulosPorPagina;
            model.NumPaginas = (int)Math.Truncate(numPag) + 1;
            int skip= (model.PaginaActual - 1)*articulosPorPagina;
            model.ArticulosRecientes = articulos.Skip(skip).Take(articulosPorPagina).ToList();
            model.ArticulosBusqueda = articulosFiltrados.Skip(skip).Take(articulosPorPagina).ToList();

            model.CategoriaId = categoria.CategoriaId;
            model.Categoria = categoria;

            return View(model);
        }
        

        private TagViewModel GenerateTagModel(Tag tag, int? page)
        {
            var fechaActual = DateTime.Now;
            TagViewModel model = new TagViewModel();
      
            var articulos = db.Articulos.Include(a => a.Categoria).Where(art => art.Publicado && art.FechaPublicacion <= fechaActual)
             .OrderByDescending(art => art.FechaPublicacion);
            var articulosFiltrados = tag.Articulos.Where(art => art.Publicado && art.FechaPublicacion <= fechaActual).OrderByDescending(art => art.FechaPublicacion);

            if (page.HasValue)
            {
                model.PaginaActual = page.Value;
            }
            else
            {
                model.PaginaActual = 1;
            }
            double numPag = articulosFiltrados.Count() / articulosPorPagina;
            model.NumPaginas = (int)Math.Truncate(numPag) +1;
            int skip = (model.PaginaActual - 1) * articulosPorPagina;
            model.ArticulosRecientes = articulos.Skip(skip).Take(articulosPorPagina).ToList();
            model.ArticulosBusqueda = articulosFiltrados.Skip(skip).Take(articulosPorPagina).ToList();


            model.TagId = tag.TagId;
            model.Tag = tag;
            return model;
        }

        // GET: Publico/Articulos/Tag/1
        public ActionResult Tag(string id, int? page)
        {
            var tag = db.Tags.Include("Articulos.Categoria").FirstOrDefault(ta=>ta.DescripcionSeo==id);
            if (tag == null)
            {
                return HttpNotFound();
            }
            TagViewModel model = GenerateTagModel(tag, page);

            return View(model);
        }

        // GET: Publico/Articulos/Categoria/1
        public ActionResult Buscar(string id,int? page)
        {
            var fechaActual = DateTime.Now;
            BuscarViewModel model = new BuscarViewModel();

 
            var articulos = db.Articulos.Include(a => a.Categoria).Where(art => art.Publicado && art.FechaPublicacion <= fechaActual)
              .OrderByDescending(art => art.FechaPublicacion);
            var articulosFiltrados = articulos.Where(art => art.Titulo.Contains(id) || art.Cuerpo.Contains(id));

            if (page.HasValue)
            {
                model.PaginaActual = page.Value;
            }
            else
            {
                model.PaginaActual = 1;
            }
            double numPag = articulosFiltrados.Count() / articulosPorPagina;
            model.NumPaginas = (int)Math.Truncate(numPag) + 1;
            int skip = (model.PaginaActual - 1) * articulosPorPagina;
            model.ArticulosRecientes = articulos.Skip(skip).Take(articulosPorPagina).ToList();
            model.ArticulosBusqueda = articulosFiltrados.Skip(skip).Take(articulosPorPagina).ToList();



            model.Texto = id;

            return View(model);
        }

        // GET: Publico/Articulos
        public ActionResult About()
        {
            ArticuloBaseViewModel model = new ArticuloBaseViewModel();
            return View(model);
        }

        // POST: Admin/Articulos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
   
        public string Comentario(int ArticuloId,string email, string nombre, string texto)
        {
            if (string.IsNullOrWhiteSpace(email)|| string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(texto) )
            {
                return "You must fill all the fields";
            }
            var articulo = db.Articulos.Find(ArticuloId);
            if (articulo == null)
            {
                return "There has been an error saving your comment";
            }
            if (ModelState.IsValid)
            {
                Comentario comentario = new Comentario()
                {
                    Aceptado = false,
                    ArticuloId = ArticuloId,
                    Email = email,
                    FechaHoraComentario = DateTime.Now,
                    Nombre = nombre,
                    Texto = texto
                };
                

                db.Comentarios.Add(comentario);
                db.SaveChanges();

                
                return "Your comment has been successfully saved, it is waiting to be moderate";

            }
            return "There has been an error saving your comment";
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
