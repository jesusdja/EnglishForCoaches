using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EnglishForCoachesWeb.Database.Varios
{
    public class AccesoUsuario
    {
        public int AccesoUsuarioId { get; set; }

        public string UsuarioId { get; set; }

        public string Usuario { get; set; }


        [Display(Name = "Fecha acceso")]
        public DateTime FechaAcceso { get; set; }


        [Display(Name = "IP")]
        public string Ip { get; set; }

    }
}