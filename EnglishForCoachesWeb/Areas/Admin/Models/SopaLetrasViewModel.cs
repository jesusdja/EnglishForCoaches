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
using EnglishForCoachesWeb.Database.JuegoOnline;

namespace EnglishForCoachesWeb.Areas.Admin.Models
{
    public class SopaLetrasViewModel
    {
        public SopaLetras SopaLetras { get; set; }
        public JuegoOnline juegoOnline { get; set; }
        public string Comentarios { get; set; }


        public string[][] Letras { get; set; }

        public void Inicializar(int juegoOnlineId)
        {
            Letras = new string[12][];
            AuthContext db = new AuthContext();

            juegoOnline = db.JuegoOnlines.Include(s => s.SubTema.Tema).Include(s => s.SopaLetras).Include(s => s.TipoJuegoOnline).SingleOrDefault(bl => bl.JuegoOnlineId == juegoOnlineId);
        }
    }

    public class SopaLetrasCreateViewModel : SopaLetrasViewModel
    {
    }

    public class SopaLetrasEditViewModel : SopaLetrasViewModel
    {

    }
}
