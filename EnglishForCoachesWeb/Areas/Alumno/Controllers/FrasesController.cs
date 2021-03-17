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
using System.Security.Claims;
using EnglishForCoachesWeb.DataAccess;

namespace EnglishForCoachesWeb.Areas.Alumno.Controllers
{
    [Authorize(Roles = "Alumno,Admin")]
    public class FrasesController : MVCBaseController
    {
        private AuthContext db = new AuthContext();
        
        // GET: Admin/Contenidos
        public ActionResult Index(int id, int gramaticaId)
        {
            if (id == null|| gramaticaId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var subtema = SubTemaDataAccess.ObtenerSubTemas(db).Include(s => s.Tema).SingleOrDefault(s => s.SubTemaId == id);
            var gramatica = GramaticaDataAccess.ObtenerGramaticas(db).FirstOrDefault(g=>g.GramaticaId==gramaticaId);
            if (subtema == null|| gramatica==null)
            {
                return HttpNotFound();
            }
            if (!ComprobarAccesoSubTema(id))
            {
                return RedirectToAction("SinAcceso", "Home", new { Area = "Alumno" });
            }



            var userId = ((ClaimsIdentity)User.Identity).FindFirst("UserId").Value;
            var frases = db.Frases.Where(fr=>fr.GramaticaId==gramaticaId).OrderBy(v => v.Palabra_en).ToList();
       

            FrasesIndexViewModel viewModel = new FrasesIndexViewModel();
            viewModel.glosario = db.FraseGlosarios.Where(vg => vg.AlumnoId == userId).Select(vg => vg.FraseId).ToList();
            viewModel.frases = frases;
            viewModel.Subtema = subtema;
            viewModel.Gramatica = gramatica;

            return View(viewModel);
        }



        // GET: Alumno/Vocabularios/Glosario
        public ActionResult Glosario( )
        {
            FrasesIndexViewModel viewModel = new FrasesIndexViewModel();
            var userId = ((ClaimsIdentity)User.Identity).FindFirst("UserId").Value;

            viewModel.glosario = db.FraseGlosarios.Where(vg => vg.AlumnoId == userId).Select(vg => vg.FraseId).ToList();

            var frases = db.Frases.OrderBy(v => v.Palabra_en);
            viewModel.frases = frases.Where(v => viewModel.glosario.Contains(v.FraseId)).ToList();

            return View(viewModel);
        }


        public JsonResult AddGlosario(int id)
        {
            var userId = ((ClaimsIdentity)User.Identity).FindFirst("UserId").Value;
            string resultado = "";
            FraseGlosario existe = db.FraseGlosarios.FirstOrDefault(vg => vg.FraseId == id && vg.AlumnoId == userId);
            if (existe != null)
            {
                db.FraseGlosarios.Remove(existe);
                db.SaveChanges();
                resultado = "Eliminado";
            }
            else
            {
                FraseGlosario fraseGlosario = new FraseGlosario()
                {
                    AlumnoId = userId,
                    Fecha = DateTime.Now,
                    FraseId = id
                };
                db.FraseGlosarios.Add(fraseGlosario);
                db.SaveChanges();
                resultado = "Añadido";
            }

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
    }
}
