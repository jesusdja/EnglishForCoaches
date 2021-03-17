using EnglishForCoachesWeb.Database;
using EnglishForCoachesWeb.Database.Ejercicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using EnglishForCoachesWeb.Validation;
using System.Web;

namespace EnglishForCoachesWeb.Areas.Admin.Models
{
    public class TestViewModel
    {
        [Display(Name = "Imagen")]
        [MaxFileSize(5 * 1024 * 1024, ErrorMessage = "El tamaño máximo permitido es de {0} bytes")]
        [MinImageSizeAttribute(300, 300, ErrorMessage = "El tamaño mínimo de la imagen es de {0}x{1} pixels")]
        public virtual HttpPostedFileBase ImageFile { get; set; }
        public Test Test { get; set; }
        public Bloque bloque { get; set; }


        public void Inicializar(int bloqueId)
        {
            AuthContext db = new AuthContext();

            bloque = db.Bloques.Include(s => s.SubTema.Tema).Include(s => s.Tests).Include(s => s.TipoEjercicio).SingleOrDefault(bl => bl.BloqueId == bloqueId);

        }
    }

    public class TestCreateViewModel : TestViewModel
    {
    }

    public class TestEditViewModel : TestViewModel
    {
        public int? ExamenId { get; set; }
    }
}
