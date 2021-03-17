using EnglishForCoachesWeb.Database;
using EnglishForCoachesWeb.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnglishForCoachesWeb.Areas.Publico.Models
{

    public class ArticuloBaseViewModel
    {
        public class TagNube
        {
            public Tag Tag { get; set; }
            public int Size { get; set; }
        }

        public List<Categoria> Categorias { get; set; }
        public List<TagNube> TagsNube { get; set; }

        public ArticuloBaseViewModel()
        {
            AuthContext db = new AuthContext();
            int b = 1 + 2;
            Categorias = db.Categorias.ToList();

            var listaTagsNube = new List<TagNube>();
            var tags = db.Articulos.Where(art=>art.FechaPublicacion<=DateTime.Now&&art.Publicado).Select(art=>art.Tags).SelectMany(tag=>tag).Distinct();
            var cantidades = tags.Select(tag => tag.Articulos.Count).Distinct().OrderBy(cantidad => cantidad).ToArray();

            var ncantidades = cantidades.Length;
            for (int i = 0; i < ncantidades; i++)
            {
                var cantidad = cantidades[i];
                //i va de 0 a length-1
                double equivalencia = (i+1) * 5 / ncantidades;
                int size = (int)Math.Round(equivalencia, 0);

                var tagsCantidad = tags.Where(tag => tag.Articulos.Count == cantidad);
                foreach (var tag in tagsCantidad)
                {
                    listaTagsNube.Add(new TagNube()
                    {
                        Tag = tag,
                        Size = size
                    });
                }
            }
            TagsNube = listaTagsNube.OrderBy(ltn => ltn.Tag.Descripcion).ToList();
        }
    }

    public class ArticuloDetailViewModel: ArticuloBaseViewModel
    {
        public Articulo Articulo { get; set; }
        public List<Articulo> ArticulosRecientes { get; set; }

        public int ArticuloId { get; set; }
        [Display(Name = "Comentario")]
        [RequiredLocalized]
        public string TextoComentario { get; set; }

        [Display(Name = "Nombre")]
        [RequiredLocalized]
        public string NombreComentario { get; set; }

        [Display(Name = "E-mail")]
        [RequiredLocalized]
        public string EmailComentario { get; set; }
    }

    public class IndexViewModel: ArticuloBaseViewModel
    {
        public List<Articulo> ArticulosDestacados { get; set; } //3
        public List<Articulo> ArticulosLista { get; set; }//8
    }

    public class ArticulosViewModel: ArticuloBaseViewModel
    {
        public List<Articulo> ArticulosBusqueda { get; set; }
        public List<Articulo> ArticulosRecientes { get; set; }

        public int NumPaginas { get; set; }
        public int PaginaActual { get; set; }
    }

    public class CategoriaViewModel : ArticulosViewModel
    {
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }
    }

    public class BuscarViewModel : ArticulosViewModel
    {
        public string Texto { get; set; }
    }

    public class TagViewModel : ArticulosViewModel
    {
        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}