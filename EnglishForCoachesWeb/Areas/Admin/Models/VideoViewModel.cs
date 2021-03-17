using EnglishForCoachesWeb.Database;
using EnglishForCoachesWeb.Database.Ejercicios;
using EnglishForCoachesWeb.Database.Varios;
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
    public class VideoIndexViewModel : ViewModelPaginado
    {
        public List<Video> listadoVideos { get; set; }
   
        public string TextoBusqueda { get; set; }

    }


        public class VideoViewModel
    {


        public Video Video { get; set; }
        
        
    }

    public class VideoCreateViewModel: VideoViewModel
    {
        [RequiredLocalized]
        [Display(Name = "Fichero adjunto")]
        [MaxFileSize(300 * 1024 * 1024, ErrorMessage = "El tamaño máximo permitido es de {0} bytes")]
        public virtual HttpPostedFileBase File { get; set; }
    }

        public class VideoEditViewModel: VideoViewModel
    {

        [Display(Name = "Fichero adjunto")]
        [MaxFileSize(5 * 1024 * 1024, ErrorMessage = "El tamaño máximo permitido es de {0} bytes")]
        public virtual HttpPostedFileBase File { get; set; }
    }
}