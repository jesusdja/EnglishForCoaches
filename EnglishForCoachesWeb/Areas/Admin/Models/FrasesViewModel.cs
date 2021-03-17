using EnglishForCoachesWeb.Database;
using EnglishForCoachesWeb.Database.Ejercicios;
using EnglishForCoachesWeb.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Data.Entity;

namespace EnglishForCoachesWeb.Areas.Admin.Models
{
    public class FrasesIndexViewModel
    {
        public List<Frase> listadoFrases { get; set; }

        public Gramatica Gramatica { get; set; }
        
    }

    public class FrasesGestorViewModel: ViewModelPaginado
    {

        public List<Frase> listadoFrases { get; set; }
        public string TextoBusqueda { get; set; }

        public int? TemaId { get; set; }
        public int? SubtemaId { get; set; }
        public int? GramaticaId { get; set; }

        public SelectList temaSelectList { get; set; }
        public SelectList subtemaSelectList { get; set; }
        public SelectList gramaticaSelectList { get; set; }

        public SelectList vocabularioSelectList { get; set; }
        public void InicializarDesplegables()
        {
            AuthContext db = new AuthContext();

            temaSelectList = new SelectList(db.Temas.OrderBy(a => a.TemaId), "TemaId", "Descripcion");
            subtemaSelectList = new SelectList(db.SubTemas.OrderBy(t =>t.TemaId).ThenBy(t=> t.Orden), "SubtemaId", "Descripcion");
            gramaticaSelectList = new SelectList(db.Gramaticas.Include(gr => gr.SubTema).OrderBy(t => t.SubTema.TemaId).ThenBy(t => t.SubTema.Orden).ThenBy(t => t.Orden), "GramaticaId", "Titulo");

            vocabularioSelectList = new SelectList(db.Vocabularios.Select(vo => new
            {
                VocabularioId = vo.VocabularioId,
                Descripcion = vo.Palabra_es + " - " + vo.Palabra_en
            }).OrderBy(vo => vo.Descripcion), "VocabularioId", "Descripcion");
        }
    }

    public class FrasesViewModel
    {
        public string GramaticaTitulo { get; set; }
        public SubTema SubTema { get; set; }
        public int GramaticaId { get; set; }
        public Frase Frase { get; set; }

        [Display(Name = "Fichero de audio en español")]
        [MaxFileSize(5 * 1024 * 1024, ErrorMessage = "El tamaño máximo permitido es de {0} bytes")]
        public virtual HttpPostedFileBase AudioFile_es { get; set; }

        [Display(Name = "Fichero de audio en ingles")]
        [MaxFileSize(5 * 1024 * 1024, ErrorMessage = "El tamaño máximo permitido es de {0} bytes")]
        public virtual HttpPostedFileBase AudioFile_en { get; set; }

        public SelectList vocabularioSelectList { get; set; }

        public string src { get; set; }

        public SelectList gramaticaSelectList { get; set; }


        //Mantener busqueda
        public string TextoBusqueda { get; set; }
        public int? TemaIdBusqueda { get; set; }
        public int? SubtemaIdBusqueda { get; set; }
        public int? GramaticaIdBusqueda { get; set; }
        public int? PaginaBusqueda { get; set; }
               
        public void InicializarDesplegables()
        {
            AuthContext db = new AuthContext();

            gramaticaSelectList = new SelectList(db.Gramaticas.Include(gr => gr.SubTema.Tema).Select(gr => new
            {
                GramaticaId = gr.GramaticaId,
                FullName = gr.SubTema.Tema.Descripcion + "/" + gr.SubTema.Descripcion + "/" + gr.Titulo,
                Orden1 = gr.SubTema.Tema.TemaId,
                Orden2 = gr.SubTema.Orden,
                Orden3 = gr.Orden,
            }).OrderBy(t => t.Orden1).ThenBy(t => t.Orden2).ThenBy(t => t.Orden3), "GramaticaId", "FullName");

            vocabularioSelectList = new SelectList(db.Vocabularios.Select(vo => new
            {
                VocabularioId = vo.VocabularioId,
                Descripcion=vo.Palabra_es + " - "+vo.Palabra_en
            }).OrderBy(vo => vo.Descripcion), "VocabularioId", "Descripcion");
        }
    }

    public class FrasesCreateViewModel: FrasesViewModel
    {
    }
    public class FrasesEditViewModel: FrasesViewModel
    {
        public string VocabularioIds { get; set; }
    }
    public class FrasesCopyViewModel
    {
        public Frase Frase { get; set; }

        public string GramaticaTitulo { get; set; }
        public SubTema SubTema { get; set; }
        public int GramaticaId { get; set; }


        public List<Tema> TemaList { get; set; }
        public List<SubTema> SubTemaList { get; set; }
        public List<Gramatica> GramaticaList { get; set; }

        public int GramaticaCopiarId { get; set; }

        public SelectList GramaticasSelectList { get; set; }
        public void InicializarDesplegables()
        {
            AuthContext db = new AuthContext();

            TemaList = db.Temas.ToList();
            SubTemaList = db.SubTemas.OrderBy(t => t.Orden).ToList();
            GramaticaList = db.Gramaticas.OrderBy(t => t.Orden).ToList();
            GramaticasSelectList = new SelectList(db.Gramaticas.OrderBy(t => t.Orden), "GramaticaId", "Titulo");
        }
    }
}