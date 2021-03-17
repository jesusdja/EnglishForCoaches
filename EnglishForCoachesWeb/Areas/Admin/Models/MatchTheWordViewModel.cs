using EnglishForCoachesWeb.Database;
using EnglishForCoachesWeb.Database.Ejercicios;
using EnglishForCoachesWeb.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace EnglishForCoachesWeb.Areas.Admin.Models
{   

    public class MatchTheWordViewModel
    {
        public MatchTheWord MatchTheWord { get; set; }

        public Bloque bloque { get; set; }

        public void Inicializar(int bloqueId)
        {
            AuthContext db = new AuthContext();

            bloque = db.Bloques.Include(s => s.SubTema.Tema).Include(s => s.MatchTheWords).Include(s => s.TipoEjercicio).SingleOrDefault(bl => bl.BloqueId == bloqueId);
        }
    }

    public class MatchTheWordCreateViewModel : MatchTheWordViewModel
    {
    }

    public class MatchTheWordEditViewModel : MatchTheWordViewModel
    {
       
    }
}