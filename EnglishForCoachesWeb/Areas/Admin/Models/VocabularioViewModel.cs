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

namespace EnglishForCoachesWeb.Areas.Admin.Models
{
    public class VocabularioIndexViewModel : ViewModelPaginado
    {
        public List<Vocabulario> listadoVocabularios { get; set; }

        public string TextoBusqueda { get; set; }

    }

    public class VocabularioViewModel
    {

        [Display(Name = "Fichero de audio")]
        [MaxFileSize(5 * 1024 * 1024, ErrorMessage = "El tamaño máximo permitido es de {0} bytes")]
        public virtual HttpPostedFileBase AudioFile { get; set; }

        public Vocabulario Vocabulario { get; set; }

        
        public SelectList categoriaVocabulariosSelectList { get; set; }
        public void InicializarDesplegables()
        {
            AuthContext db = new AuthContext();

            categoriaVocabulariosSelectList = new SelectList(db.CategoriaVocabularios.OrderBy(t => t.Descripcion), "CategoriaVocabularioId", "Descripcion");
        }
    }

    public class VocabularioCreateViewModel: VocabularioViewModel
    {
    }

        public class VocabularioEditViewModel: VocabularioViewModel
    {
        public string DefinicionesVocabulario { get; set; }
    }
}