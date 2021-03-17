using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnglishForCoachesWeb.Database
{
    public class GrupoUsuario
    {
        public int GrupoUsuarioId { get; set; }
        [Display(Name = "Grupo")]
        public string Descripcion { get; set; }


        public int NumeroMaximoUsuarios { get; set; }

    }

    public class GrupoUsuarioSeleccionado
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public bool Seleccionado { get; set; }
    }


    public enum GruposUsuariosId
    {
        TecnicosRFEF = 1,
        EscuelaNacional = 2,
        InvitadosEscuela = 3,
        StaffRFEF = 4,
        VIPGuest = 5,
        ComiteEntrenadores = 6,
        FUTSAL = 7
            //Silvia puede crear nuevos
    }
}