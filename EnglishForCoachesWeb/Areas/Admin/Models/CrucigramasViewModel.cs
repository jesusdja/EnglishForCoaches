using EnglishForCoachesWeb.Database;
using EnglishForCoachesWeb.Database.Ejercicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace EnglishForCoachesWeb.Areas.Admin.Models
{
    public class CrucigramaViewModel
    {
        public Crucigrama Crucigrama { get; set; }
        public Bloque bloque { get; set; }

        
        public string[][] Letras { get; set; }

        public void Inicializar(int bloqueId)
        {
            Letras = new string[12][];
            AuthContext db = new AuthContext();

            bloque = db.Bloques.Include(s => s.SubTema.Tema).Include(s => s.Crucigramas).Include(s => s.TipoEjercicio).SingleOrDefault(bl => bl.BloqueId == bloqueId);
        }
    }

    public class CrucigramaCreateViewModel : CrucigramaViewModel
    {
    }

    public class CrucigramaEditViewModel : CrucigramaViewModel
    {

    }
}
