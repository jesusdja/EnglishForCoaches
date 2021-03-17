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
    public class SubTemasController : MVCBaseController
    {
        private AuthContext db = new AuthContext();
        
        //Para el menu
        public ActionResult ListarTemasAlumno()
        {
            var temas = TemaDataAccess.ObtenerTemas(db).OrderBy(d => d.TemaId);
            return PartialView("~/Areas/Alumno/Views/Shared/_ListarTemasAlumno.cshtml", temas.ToList());
        }
        // GET: Admin/SubTemas
        public ActionResult Index(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var tema = TemaDataAccess.ObtenerTemas(db).FirstOrDefault(te=>te.TemaId==id);
            if (tema == null)
            {
                return HttpNotFound();
            }


            var userId = ((ClaimsIdentity)User.Identity).FindFirst("UserId").Value;

            var subTemas = SubTemaDataAccess.ObtenerSubTemas(db).Include(s => s.Tema).Where(subtema => subtema.TemaId == id).OrderBy(d => d.Orden).ToList();
            var listaSubtemaIds = subTemas.Select(st => st.SubTemaId).ToList();

            var subtemasDesbloqueados = db.SubTemaDesbloqueados.Where(sd => sd.AlumnoId == userId &&
            listaSubtemaIds.Contains(sd.SubTemaId)).Select(sd => sd.SubTemaId).ToList();

            SubTemasIndexViewModel viewModel = new SubTemasIndexViewModel();
            viewModel.Tema = tema;


            viewModel.listadoSubTemasBloqueados = subTemas.Where(st=>!subtemasDesbloqueados.Contains(st.SubTemaId)).ToList();
            viewModel.listadoSubTemasDesbloqueados = subTemas.Where(st => subtemasDesbloqueados.Contains(st.SubTemaId)).ToList();
            if (User.IsInRole("Admin"))
            {
                viewModel.listadoSubTemasBloqueados = new List<SubTema>();
                viewModel.listadoSubTemasDesbloqueados = subTemas.ToList();
            }


                var gramaticas = GramaticaDataAccess.ObtenerGramaticas(db).Include(gt => gt.Vocabularios).Where(gr => listaSubtemaIds.Contains(gr.SubTemaId));
            var gramaticasIds = gramaticas.Select(gr => gr.GramaticaId).ToList();

            viewModel.NUnidades = gramaticasIds.Count;
            viewModel.NFrases = db.Frases.Count(f => gramaticasIds.Contains(f.GramaticaId));
            viewModel.NVocabulario = gramaticas.SelectMany(gr=> gr.Vocabularios).Count();
            viewModel.NPracticas = BloqueDataAccess.ObtenerBloques(db).Count(b => listaSubtemaIds.Contains(b.SubTemaId));
            viewModel.NLecciones = listaSubtemaIds.Count;

            viewModel.TemaSuperado = (db.Examenes.Count(e => e.AlumnoId == userId && e.Aprobado && e.SubTema.TemaId == id) == listaSubtemaIds.Count);

            viewModel.examenesDesbloqueados = db.ExamenDesbloqueados.Where(ex => ex.AlumnoId == userId).Select(ex => ex.SubTemaId).ToList();
            viewModel.listadoSubTemasAcceso = db.SubTemaAccesoUsuarios.Where(sau => sau.AlumnoId == userId).Select(sau => sau.SubTemaId).ToList();

            return View(viewModel);
        }

        // GET: Admin/SubTemas
        public ActionResult View(int id,int? gramaticaId)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var subTema = SubTemaDataAccess.ObtenerSubTemas(db).FirstOrDefault(su=>su.SubTemaId==id);
            if (subTema == null)
                return HttpNotFound();

            if (!ComprobarAccesoSubTema(id))
                return RedirectToAction("SinAcceso", "Home", new { Area = "Alumno" });

            SubTemasViewViewModel viewModel = new SubTemasViewViewModel();
            viewModel.Tema = TemaDataAccess.ObtenerTemas(db).FirstOrDefault(te=>te.TemaId==subTema.TemaId);
            if (viewModel.Tema == null)
                return HttpNotFound();
            viewModel.SubTema = subTema;

            var bloques = BloqueDataAccess.ObtenerBloques(db).Include(t => t.Area).Include(t => t.TipoEjercicio).Where(t => t.SubTemaId == id);
            var areas = db.Areas.Where(a=>a.Orden.HasValue).OrderBy(a=>a.Orden.Value).ToList();

            var listadoAreas = new List<AreaContenidos>();
            foreach (var area in areas)
                listadoAreas.Add(new AreaContenidos
                {
                    Area = area,
                    NumContenidos = bloques.Where(c => c.AreaId == area.AreaId).Count()
                });
            viewModel.listadoAreas = listadoAreas;

            var Gramaticas = GramaticaDataAccess.ObtenerGramaticas(db).Include(gram => gram.GramaticaCuerpo).Where(gram => gram.SubTemaId == id).OrderBy(d => d.Orden).ToList();
            viewModel.Gramaticas=Gramaticas;
            if (gramaticaId.HasValue)
                viewModel.GramaticaMostrar = GramaticaDataAccess.ObtenerGramaticas(db).Include(gram => gram.GramaticaCuerpo).FirstOrDefault(gr=>gr.GramaticaId==gramaticaId.Value);
            else
                viewModel.GramaticaMostrar = Gramaticas.FirstOrDefault();

            var userId = ((ClaimsIdentity)User.Identity).FindFirst("UserId").Value;

            viewModel.MostrarExamen = (db.Examenes.Where(e => e.AlumnoId == userId && e.SubTemaId == id && e.Aprobado).FirstOrDefault() == null);
            viewModel.examenesDesbloqueados = db.ExamenDesbloqueados.Where(ex => ex.AlumnoId == userId && ex.SubTemaId==id).Select(ex => ex.SubTemaId).ToList();


            return View(viewModel);
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
