using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EnglishForCoachesWeb.Database;
using EnglishForCoachesWeb.Database.Ejercicios;
using EnglishForCoachesWeb.Areas.Alumno.Models;
using EnglishForCoachesWeb.Controllers;
using EnglishForCoachesWeb.Helpers;
using System.Security.Claims;
using EnglishForCoachesWeb.Auth;
using EnglishForCoachesWeb.DataAccess;

namespace EnglishForCoachesWeb.Areas.Alumno.Controllers
{
    [Authorize(Roles = "Alumno,Admin")]
    public class BloquesController : MVCBaseController
    {
        private AuthContext db = new AuthContext();
        
        // GET: Admin/Contenidos
        public ActionResult Index(int id,int areaId)
        {
            if (id == null|| areaId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var subtema = SubTemaDataAccess.ObtenerSubTemas(db).Include(s => s.Tema).SingleOrDefault(s => s.SubTemaId == id);
            var area = db.Areas.SingleOrDefault(s => s.AreaId == areaId);
            if (subtema == null || area == null)
            {
                return HttpNotFound();
            }
            if (!ComprobarAccesoSubTema(id))
            {
                return RedirectToAction("SinAcceso", "Home", new { Area = "Alumno" });
            }

            var realizados=ContenidoHelper.ObtenerEjercicioHecho(id, areaId);
            var conMistakes = ContenidoHelper.ObtenerEjerciciosConMistakes(id, areaId);
            

            var bloques = BloqueDataAccess.ObtenerBloques(db).Include(t=>t.TipoEjercicio).Where(t => t.SubTemaId== id && t.AreaId== areaId).ToList();
            var bloquesIds = bloques.Select(bl => bl.BloqueId).ToList();


            BloquesIndexViewModel viewModel = new BloquesIndexViewModel();
            viewModel.listadoBloques = bloques;
            var userId = ((ClaimsIdentity)User.Identity).FindFirst("UserId").Value;
            viewModel.bloquesDesbloqueados = db.BloqueDesbloqueados.Where(ex => ex.AlumnoId == userId && bloquesIds.Contains(ex.BloqueId)).Select(ex => ex.BloqueId).ToList();


            viewModel.Subtema = subtema;
            viewModel.Area = area;
            viewModel.realizados = realizados.Select(br=>br.BloqueId).ToList();
            viewModel.conMistakes = conMistakes.Select(br => br.BloqueId).ToList();
            

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

                BloqueDesbloqueado BloqueDes = new BloqueDesbloqueado()
                {
                    AlumnoId = userId,
                    BloqueId = id,
                    FechaDesbloqueo = DateTime.Now
                };

                db.BloqueDesbloqueados.Add(BloqueDes);
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
