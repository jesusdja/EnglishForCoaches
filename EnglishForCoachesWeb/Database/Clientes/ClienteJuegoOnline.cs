using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnglishForCoachesWeb.Database.Clientes
{
    public class ClienteJuegoOnline
    {
        public int ClienteJuegoOnlineId { get; set; }
        public int ClienteId { get; set; }
        public int JuegoOnlineId { get; set; }
    }
}