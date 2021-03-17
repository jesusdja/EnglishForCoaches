using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

using EnglishForCoachesWeb.Validation;

namespace EnglishForCoachesWeb.Database.Ejercicios
{
    public class Mistake
    {
        public int MistakeId { get; set; }

        public string AlumnoId { get; set; }
        public int BloqueId { get; set; }    
        public int PreguntaId { get; set; }        

    }
}