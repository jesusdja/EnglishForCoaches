using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnglishForCoachesWeb.Database.Ejercicios
{
    public class CasillaCrucigrama
    {
        public int CasillaCrucigramaId { get; set; }



        public int PosV { get; set; }
        public int PosH { get; set; }


        public string letra { get; set; }


        
    }
}