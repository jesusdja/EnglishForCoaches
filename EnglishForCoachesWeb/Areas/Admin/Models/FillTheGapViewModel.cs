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
    public class FillTheGapViewModel
    {
        public FillTheGap FillTheGap { get; set; }
        public Bloque bloque { get; set; }    

        public void Inicializar(int bloqueId)
        {
            AuthContext db = new AuthContext();

            bloque = db.Bloques.Include(s => s.SubTema.Tema).Include(s => s.FillTheGaps).Include(s => s.TipoEjercicio).SingleOrDefault(bl => bl.BloqueId == bloqueId);
        }
    }

    public class FillTheGapCreateViewModel : FillTheGapViewModel
    {
    }

    public class FillTheGapEditViewModel : FillTheGapViewModel
    {

        public int? ExamenId { get; set; }
    }
}
