using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EnglishForCoachesWeb.Database;
using EnglishForCoachesWeb.Database.JuegoOnline;
using EnglishForCoachesWeb.Areas.Alumno.Models;
using EnglishForCoachesWeb.Controllers;
using EnglishForCoachesWeb.Helpers;
using System.Security.Claims;
using EnglishForCoachesWeb.Auth;
using EnglishForCoachesWeb.DataAccess;

namespace EnglishForCoachesWeb.Areas.Alumno.Controllers
{
    [Authorize(Roles = "Alumno,Admin")]
    public class JuegoOnlinesController : MVCBaseController
    {
        private AuthContext db = new AuthContext();
        
        // GET: Admin/Contenidos
        public ActionResult Index(int id )
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var subtema = SubTemaDataAccess.ObtenerSubTemas(db).Include(s => s.Tema).SingleOrDefault(s => s.SubTemaId == id);
            if (subtema == null )
            {
                return HttpNotFound();
            }
            if (!ComprobarAccesoSubTema(id))
            {
                return RedirectToAction("SinAcceso", "Home", new { Area = "Alumno" });
            }

            var realizados=JuegoOnlineHelper.ObtenerJuegoOnlineHecho(id);            

            var JuegoOnlines = JuegoOnlineDataAccess.ObtenerJuegoOnlines(db).Include(t=>t.TipoJuegoOnline).Where(t => t.SubTemaId== id).ToList();
            var JuegoOnlinesIds = JuegoOnlines.Select(bl => bl.JuegoOnlineId).ToList();


            JuegoOnlinesIndexViewModel viewModel = new JuegoOnlinesIndexViewModel();
            viewModel.listadoJuegoOnlines = JuegoOnlines;
            var userId = ((ClaimsIdentity)User.Identity).FindFirst("UserId").Value;
            viewModel.juegoOnlinesDesbloqueados = db.JuegoOnlineDesbloqueados.Where(ex => ex.AlumnoId == userId && JuegoOnlinesIds.Contains(ex.JuegoOnlineId)).Select(ex => ex.JuegoOnlineId).ToList();

            viewModel.Subtema = subtema;
            
            viewModel.realizados = realizados.Select(br=>br.JuegoOnlineId).ToList();            

            return View(viewModel);
        }


        public JsonResult Desbloquear(int id)
        {
            var userId = ((ClaimsIdentity)User.Identity).FindFirst("UserId").Value;

            AuthRepository authRepository = new AuthRepository();
            ApplicationUser user = authRepository.FindByName(User.Identity.Name);
            string resultado = "";
            if (user.PuntosActual >= 5)
            {
                user.PuntosActual = user.PuntosActual - 5;

                var userResult = authRepository.Update(user);

                JuegoOnlineDesbloqueado JuegoOnlineDes = new JuegoOnlineDesbloqueado()
                {
                    AlumnoId = userId,
                    JuegoOnlineId = id,
                    FechaDesbloqueo = DateTime.Now
                };

                db.JuegoOnlineDesbloqueados.Add(JuegoOnlineDes);
                db.SaveChanges();
                resultado = "Desbloqueado";
            }
            else
            {
                resultado = "NoPuntos";
            }


            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

    }
}
