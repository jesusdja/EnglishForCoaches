using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnglishForCoachesWeb.Database.Ejercicios
{
    public class ContenidoBase
    {
        public int Id { get; set; }

        [Display(Name = "Descripcion")]
        public string Descripcion { get; set; }


        [Display(Name = "SubTema")]
        public int SubTemaId { get; set; }
        public virtual SubTema SubTema { get; set; }

        [Display(Name = "Area")]
        public int AreaId { get; set; }
        public virtual Area Area { get; set; }
        
        [Display(Name = "TipoEjercicio")]
        public int TipoEjercicioId { get; set; }
        public virtual TipoEjercicio TipoEjercicio { get; set; }


        [Display(Name = "Bloque")]
        public int BloqueId { get; set; }
        public virtual Bloque Bloque { get; set; }
    }
}