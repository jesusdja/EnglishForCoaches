using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EnglishForCoachesWeb.Database.Ejercicios
{
    public class ExamenDesbloqueado
    {
        public int ExamenDesbloqueadoId { get; set; }
        public int SubTemaId { get; set; }
        public virtual Examen Examen { get; set; }
                

        [Display(Name = "Fecha desbloqueo")]
        public DateTime FechaDesbloqueo { get; set; }
        

        public string AlumnoId { get; set; }
        
    }
}