using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnglishForCoachesWeb.Database
{
    public class TipoUsuario
    {
        public int TipoUsuarioId { get; set; }
        [Display(Name = "Tipo")]
        public string Descripcion { get; set; }
        
    }

    public class TipoUsuarioSeleccionado
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public bool Seleccionado { get; set; }
    }


    public enum TiposUsuariosId
    {
        Alumno = 1,
        AdministradorGrupo = 2,
    }
}