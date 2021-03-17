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

namespace EnglishForCoachesWeb.Areas.Admin.Models
{
    public class SkillwiseViewModel
    {
        public Skillwise Skillwise { get; set; }
        public Bloque bloque { get; set; }      

        public void Inicializar(int bloqueId)
        {
            AuthContext db = new AuthContext();

            bloque = db.Bloques.Include(s => s.SubTema.Tema).Include(s => s.Skillwises).Include(s => s.TipoEjercicio).SingleOrDefault(bl => bl.BloqueId == bloqueId);
        }
    }

    public class SkillwiseCreateViewModel : SkillwiseViewModel
    {
    }

    public class SkillwiseEditViewModel : SkillwiseViewModel
    {

    }
}
