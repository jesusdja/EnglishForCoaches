using EnglishForCoachesWeb.Database.Ejercicios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnglishForCoachesWeb.Database.JuegoOnline
{
    public class VocabularioSopaLetras
    {
        public int VocabularioSopaLetrasId { get; set; }


        [InverseProperty("VocabularioSopaLetras")]
        public SopaLetras SopaLetras { get; set; }



        public int VocabularioId { get; set; }
        public virtual Vocabulario Vocabulario { get; set; }
        public string Comentario { get; set; }



    }
}