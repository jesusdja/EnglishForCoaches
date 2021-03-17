using EnglishForCoachesWeb.Database;
using EnglishForCoachesWeb.Database.Clientes;
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
    public class ClienteIndexViewModel : ViewModelPaginado
    {
        public List<Cliente> listadoClientes { get; set; }

        public string TextoBusqueda { get; set; }

    }

    public class ClienteViewModel
    {
        [Display(Name = "Imagen de texto blanco con fondo transparente 180x56")]
        [MaxFileSize(5 * 1024 * 1024, ErrorMessage = "El tamaño máximo permitido es de {0} bytes")]
        [MinImageSizeAttribute(180, 56, ErrorMessage = "El tamaño mínimo de la imagen es de {0}x{1} pixels")]
        public virtual HttpPostedFileBase ImageFileBlanco { get; set; }

        [Display(Name = "Imagen de texto negro con fondo transparente 200x54")]
        [MaxFileSize(5 * 1024 * 1024, ErrorMessage = "El tamaño máximo permitido es de {0} bytes")]
        [MinImageSizeAttribute(200, 54, ErrorMessage = "El tamaño mínimo de la imagen es de {0}x{1} pixels")]
        public virtual HttpPostedFileBase ImageFileNegro { get; set; }

        public Cliente Cliente { get; set; }

    }

    public class ClienteCreateViewModel: ClienteViewModel
    {
    }

        public class ClienteEditViewModel: ClienteViewModel
    {
    }
}