using EnglishForCoachesWeb.Database;
using EnglishForCoachesWeb.Database.Varios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace EnglishForCoachesWeb.Areas.Admin.Models
{
    public class AccesoUsuariosIndexViewModel : ViewModelPaginado
    {
        public List<AccesoUsuario> listadoAccesoUsuarios { get; set; }
     
        
        public string TextoBusqueda { get; set; }

     
    }
    
}