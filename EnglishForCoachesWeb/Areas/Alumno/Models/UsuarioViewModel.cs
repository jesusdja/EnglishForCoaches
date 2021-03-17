using EnglishForCoachesWeb.Database;
using EnglishForCoachesWeb.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnglishForCoachesWeb.Areas.Alumno.Models
{       

    public class UsuarioEditViewModel 
    {

        [Display(Name = "Avatar")]
        [MaxFileSize(2 * 1024 * 1024, ErrorMessage = "El tamaño máximo permitido es de {0} bytes")]
        public virtual HttpPostedFileBase Avatar { get; set; }
        public ApplicationUser Usuario { get; set; }
        public string Password { get; set; }
        public string PasswordRepeat { get; set; }
    }
}