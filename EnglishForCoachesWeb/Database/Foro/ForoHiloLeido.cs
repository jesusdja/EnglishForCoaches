using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnglishForCoachesWeb.Database.Foro
{
    public class ForoHiloLeido
    {
        public int ForoHiloLeidoId { get; set; }


        public int ForoHiloId { get; set; }

        public string AlumnoId { get; set; }
    }
}