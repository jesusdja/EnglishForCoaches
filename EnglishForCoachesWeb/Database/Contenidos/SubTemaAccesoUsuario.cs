using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EnglishForCoachesWeb.Database.Ejercicios
{
    public class SubTemaAccesoUsuario
    {
        public int SubTemaAccesoUsuarioId { get; set; }
        public int SubTemaId { get; set; }
        public virtual SubTema SubTema { get; set; }
                

        [Display(Name = "Fecha acceso")]
        public DateTime FechaAcceso { get; set; }
        

        public string AlumnoId { get; set; }
        
    }
}