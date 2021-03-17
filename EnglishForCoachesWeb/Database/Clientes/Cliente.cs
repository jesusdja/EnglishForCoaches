using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnglishForCoachesWeb.Database.Clientes
{
    public class Cliente
    {
        public int ClienteId { get; set; }

        [Display(Name = "Cliente")]
        public string Descripcion { get; set; }
        public string Abreviatura { get; set; }
        public string NombreUrl { get; set; }

        public string LogoBlanco { get; set; }
        public string LogoNegro { get; set; }
        public string EstiloColor { get; set; }

    }



    public enum ClientesId
    {
        Azulita = 1
    }
}