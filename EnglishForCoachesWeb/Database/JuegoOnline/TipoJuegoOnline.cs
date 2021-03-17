using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnglishForCoachesWeb.Database.JuegoOnline
{
    public class TipoJuegoOnline
    {
        public int TipoJuegoOnlineId { get; set; }

        [Display(Name = "TipoJuegoOnline")]
        public string Descripcion { get; set; }

        public string Controlador { get; set; }
        public string EstiloColor { get; set; }
    }

    public enum TiposDeJuegosOnlineId
    {
        BeatTheGoalie= 1,
        MemoryGame = 2,
        SopaLetras = 3,
        Ahorcado = 4
    }
}