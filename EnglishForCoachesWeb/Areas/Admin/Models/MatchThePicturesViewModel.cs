using EnglishForCoachesWeb.Database;
using EnglishForCoachesWeb.Database.Ejercicios;
using EnglishForCoachesWeb.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace EnglishForCoachesWeb.Areas.Admin.Models
{   

    public class MatchThePictureViewModel
    {
        public MatchThePicture MatchThePicture { get; set; }

        public Bloque bloque { get; set; }

        public void Inicializar(int bloqueId)
        {
            AuthContext db = new AuthContext();

            bloque = db.Bloques.Include(s => s.SubTema.Tema).Include(s => s.MatchThePictures).Include(s => s.TipoEjercicio).SingleOrDefault(bl => bl.BloqueId == bloqueId);
        }
    }

    public class MatchThePictureCreateViewModel : MatchThePictureViewModel
    {
        [RequiredLocalized]
        [Display(Name = "Imagen")]
        [MaxFileSize(3 * 1024 * 1024, ErrorMessage = "El tamaño máximo permitido es de {0} bytes")]
        [MinImageSizeAttribute(50, 50, ErrorMessage = "El tamaño mínimo de la imagen es de {0}x{1} pixels")]
        public virtual HttpPostedFileBase ImageFile { get; set; }
    }

}