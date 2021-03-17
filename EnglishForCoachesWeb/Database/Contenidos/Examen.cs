using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

using EnglishForCoachesWeb.Validation;

namespace EnglishForCoachesWeb.Database.Ejercicios
{
    public class Examen
    {
        public int ExamenId { get; set; }
        public int SubTemaId { get; set; }

        public string AlumnoId { get; set; }
        public string NombreAlumno { get; set; }

        public int Aciertos { get; set; }
        public int Fallos { get; set; }
        public int Total { get; set; }

        public bool Aprobado { get; set; }

        public virtual SubTema SubTema { get; set; }

        [Display(Name = "Fecha examen")]
        public DateTime FechaExamen { get; set; }


        public virtual ICollection<RespuestaIncorrecta> RespuestasIncorrectas { get; set; }
    }
}