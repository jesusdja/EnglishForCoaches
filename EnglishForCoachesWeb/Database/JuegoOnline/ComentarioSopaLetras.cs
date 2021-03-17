using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnglishForCoachesWeb.Database.JuegoOnline
{
    public class ComentarioSopaLetras
    {
        public int ComentarioSopaLetrasId { get; set; }


        [InverseProperty("ComentarioSopaLetras")]
        public SopaLetras SopaLetras { get; set; }



        public string Palabra { get; set; }
        public string Comentario { get; set; }



    }
}