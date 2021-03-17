using EnglishForCoachesWeb.Database;
using EnglishForCoachesWeb.Database.JuegoOnline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Data;
using System.Data.Entity;

namespace EnglishForCoachesWeb.Helpers
{
    public static class JuegoOnlineHelper
    {
        public static bool MarcarJuegoOnlineHecho(int JuegoOnlineId)
        {
            AuthContext db = new AuthContext();
            var userId = ((ClaimsIdentity)HttpContext.Current.User.Identity).FindFirst("UserId").Value;

            bool noMostrarMistakes = false;
            var yaRealizado = db.JuegoOnlineRealizados.FirstOrDefault(br=>br.JuegoOnlineId==JuegoOnlineId && br.AlumnoId == userId);
            
            JuegoOnlineRealizado JuegoOnlineRealizado = new JuegoOnlineRealizado()
            {
                JuegoOnlineId = JuegoOnlineId,
                AlumnoId = userId,
                FechaRealizado = DateTime.Now
            };
            db.JuegoOnlineRealizados.Add(JuegoOnlineRealizado);
            db.SaveChanges();
            return noMostrarMistakes;
        }
        public static List<JuegoOnlineRealizado> ObtenerJuegoOnlineHecho(int subtemaId)
        {
            AuthContext db = new AuthContext();
            var userId = ((ClaimsIdentity)HttpContext.Current.User.Identity).FindFirst("UserId").Value;

            var JuegoOnlines = db.JuegoOnlines.Where(bl => bl.SubTemaId == subtemaId ).Select(bl => bl.JuegoOnlineId).ToList();

            return db.JuegoOnlineRealizados.Where(br => br.AlumnoId == userId && JuegoOnlines.Contains(br.JuegoOnlineId)).ToList();
        }
        
    }
}