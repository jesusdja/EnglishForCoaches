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
using EnglishForCoachesWeb.Auth;
using System.Security.Claims;
using EnglishForCoachesWeb.DataAccess;

namespace EnglishForCoachesWeb.Areas.Alumno.Controllers
{
    [Authorize(Roles = "Alumno,Admin")]
    public class ExtrasController : MVCBaseController
    {
        private AuthContext db = new AuthContext();

        // GET: Alumno/Extras
        public ActionResult Index(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var subtema = SubTemaDataAccess.ObtenerSubTemas(db).Include(s => s.Tema).SingleOrDefault(s => s.SubTemaId == id);
            if (subtema == null)
            {
                return HttpNotFound();
            }
            if (!ComprobarAccesoSubTema(id))
            {
                return RedirectToAction("SinAcceso", "Home", new { Area = "Alumno" });
            }

            ExtrasIndexViewModel viewModel = new ExtrasIndexViewModel();
            viewModel.categorias = db.CategoriaExtras.ToList();
            viewModel.Subtema = subtema;

            viewModel.ExtraDesbloqueadoEstadisticas = new List<ExtraDesbloqueadoEstadistica>();
            foreach (var categoria in viewModel.categorias)
            {
                var extras = ExtraDataAccess.ObtenerExtras(db).Where(ex => ex.CategoriaExtraId == categoria.CategoriaExtraId && ex.SubTemaId==id).Select(ex=>ex.ExtraId).ToList();
                int nExtras = extras.Count;
                var userId = ((ClaimsIdentity)User.Identity).FindFirst("UserId").Value;
                int nExtrasDesbloqueados = db.ExtraDesbloqueados.Count(ex => ex.AlumnoId == userId&& extras.Contains(ex.ExtraId));

                double porcentaje = (double)nExtrasDesbloqueados / nExtras * 100;
                ExtraDesbloqueadoEstadistica extraEstadistica = new ExtraDesbloqueadoEstadistica()
                {
                    CateGoriaExtraId=categoria.CategoriaExtraId,
                    NDesbloqueados = nExtrasDesbloqueados,
                    NExtras = nExtras,
                    Porcentaje = (int)porcentaje
                };
                viewModel.ExtraDesbloqueadoEstadisticas.Add(extraEstadistica);
            }

            return View(viewModel);
        }

        // GET: Alumno/Categoria/id
        public ActionResult Categoria(int id,int subtemaId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var categoria = db.CategoriaExtras.FirstOrDefault(b => b.CategoriaExtraId == id);

            if (categoria == null)
            {
                return HttpNotFound();
            }


            if (subtemaId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var subtema = SubTemaDataAccess.ObtenerSubTemas(db).Include(s => s.Tema).SingleOrDefault(s => s.SubTemaId == subtemaId);
            if (subtema == null)
            {
                return HttpNotFound();
            }
            if (!ComprobarAccesoSubTema(subtemaId))
            {
                return RedirectToAction("SinAcceso", "Home", new { Area = "Alumno" });
            }

            ExtrasCategoriaViewModel viewModel = new ExtrasCategoriaViewModel();
            viewModel.CategoriaExtra = categoria;
            viewModel.Subtema = subtema;
            viewModel.extras = ExtraDataAccess.ObtenerExtras(db).Where(ex=>ex.CategoriaExtraId==id && ex.SubTemaId== subtemaId).OrderBy(ex=>ex.Orden).ToList();

            var extras = viewModel.extras.Select(ex => ex.ExtraId).ToList();

            var userId = ((ClaimsIdentity)User.Identity).FindFirst("UserId").Value;
            viewModel.extrasDesbloqueados = db.ExtraDesbloqueados.Where(ex => ex.AlumnoId == userId && extras.Contains(ex.ExtraId)).Select(ex => ex.ExtraId).ToList();

            var siguienteExtra = viewModel.extras.Where(ex => !viewModel.extrasDesbloqueados.Contains(ex.ExtraId)).OrderBy(ex => ex.Orden).FirstOrDefault();
            if (siguienteExtra != null)
            {
                int siguienteId = siguienteExtra.ExtraId;
                viewModel.siguienteId = siguienteId;
            }
            else
            {
                viewModel.siguienteId = 0;
            }

            int nExtras = extras.Count;
            int nExtrasDesbloqueados = viewModel.extrasDesbloqueados.Count;

            double porcentaje = (double)nExtrasDesbloqueados / nExtras * 100;
            ExtraDesbloqueadoEstadistica extraEstadistica = new ExtraDesbloqueadoEstadistica()
            {
                CateGoriaExtraId = categoria.CategoriaExtraId,
                NDesbloqueados = nExtrasDesbloqueados,
                NExtras = nExtras,
                Porcentaje = (int)porcentaje
            };
            viewModel.ExtraDesbloqueadoEstadistica=extraEstadistica;


            return View(viewModel);
        }

        // GET: Alumno/View/id
        public ActionResult View(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var extra = ExtraDataAccess.ObtenerExtras(db).Include(ex=>ex.CategoriaExtra).Include(ex => ex.SubTema.Tema).FirstOrDefault(b => b.ExtraId == id);

            if (extra== null)
            {
                return HttpNotFound();
            }

            ExtrasViewViewModel viewModel = new ExtrasViewViewModel();
            viewModel.Extra = extra;
            return View(viewModel);
        }

        public JsonResult Desbloquear(int id)
        {
            var userId = ((ClaimsIdentity)User.Identity).FindFirst("UserId").Value;

            AuthRepository authRepository = new AuthRepository();
            ApplicationUser user = authRepository.FindByName(User.Identity.Name);
            string resultado = "";
            if (user.PuntosActual >= 25)
            {
                user.PuntosActual = user.PuntosActual - 25;

                var userResult = authRepository.Update(user);

                ExtraDesbloqueado extraDes = new ExtraDesbloqueado()
                {
                    AlumnoId = userId,
                    ExtraId = id,
                    FechaDesbloqueo = DateTime.Now
                };

                db.ExtraDesbloqueados.Add(extraDes);
                db.SaveChanges();
                resultado = "Desbloqueado";
            }
            else
            {
                resultado = "NoPuntos";
            }


            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        
    }
}
