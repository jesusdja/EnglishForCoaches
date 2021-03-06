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
using EnglishForCoachesWeb.Validation;
using System.ComponentModel.DataAnnotations;
using System.Web;
using EnglishForCoachesWeb.Database.JuegoOnline;

namespace EnglishForCoachesWeb.Areas.Admin.Models
{
    public class AhorcadoViewModel
    {
        public Ahorcado Ahorcado { get; set; }
        public JuegoOnline juegoOnline { get; set; }


        [Display(Name = "Fichero de audio")]
        [MaxFileSize(5 * 1024 * 1024, ErrorMessage = "El tamaño máximo permitido es de {0} bytes")]
        public virtual HttpPostedFileBase AudioFile { get; set; }

        [Display(Name = "Imagen")]
        [MaxFileSize(5 * 1024 * 1024, ErrorMessage = "El tamaño máximo permitido es de {0} bytes")]
        [MinImageSizeAttribute(300, 300, ErrorMessage = "El tamaño mínimo de la imagen es de {0}x{1} pixels")]
        public virtual HttpPostedFileBase ImageFile { get; set; }


        public void Inicializar(int juegoOnlineId)
        {
            AuthContext db = new AuthContext();

            juegoOnline = db.JuegoOnlines.Include(s => s.SubTema.Tema).Include(s => s.Ahorcado).Include(s => s.TipoJuegoOnline).SingleOrDefault(bl => bl.JuegoOnlineId == juegoOnlineId);
        }
    }

    public class AhorcadoCreateViewModel : AhorcadoViewModel
    {
    }

    public class AhorcadoEditViewModel : AhorcadoViewModel
    {

    }
}
