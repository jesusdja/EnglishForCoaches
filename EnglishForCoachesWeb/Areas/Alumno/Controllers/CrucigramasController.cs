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
using EnglishForCoachesWeb.Auth;
using EnglishForCoachesWeb.DataAccess;

namespace EnglishForCoachesWeb.Areas.Alumno.Controllers
{
    [Authorize(Roles = "Alumno,Admin")]
    public class CrucigramasController : MVCBaseController
    {
        private AuthContext db = new AuthContext();

        // GET: Admin/Contenidos
        public ActionResult Index(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var bloque = BloqueDataAccess.ObtenerBloques(db).Include(bl => bl.Area).Include(bl => bl.SubTema.Tema).FirstOrDefault(b => b.BloqueId == id);

            if (bloque == null)
            {
                return HttpNotFound();
            }
            if (!ComprobarAccesoSubTema(bloque.SubTemaId))
            {
                return RedirectToAction("SinAcceso", "Home", new { Area = "Alumno" });
            }

            ContenidoHelper.MarcarEjercicioHecho(id);

            var crucigrama = db.Crucigramas.Include(cr=> cr.CasillaCrucigramas).FirstOrDefault(sk => sk.BloqueId == id);

            CrucigramaIndexViewModel viewModel = new CrucigramaIndexViewModel();
            viewModel.bloque = bloque;
            viewModel.Crucigrama = crucigrama;
            viewModel.Letras = new string[12][];
            viewModel.LetrasRespuesta = new string[12][];
            for (int i = 0; i < 12; i++)
            {
                viewModel.Letras[i] = new string[12];
                viewModel.LetrasRespuesta[i] = new string[12];
                for (int j = 0; j < 12; j++)
                {
                    var casilla = viewModel.Crucigrama.CasillaCrucigramas.FirstOrDefault(cr => cr.PosH == j && cr.PosV == i);
                    if (casilla != null)
                    {

                        viewModel.Letras[i][j] = casilla.letra;
                    }
                    viewModel.LetrasRespuesta[i][j] = "";
                }
            }

            return View(viewModel);
        }
        public JsonResult Contestar(int id, string[][] respuestas)
        {
            var crucigrama = db.Crucigramas.Include(c=>c.CasillaCrucigramas).FirstOrDefault(sk => sk.BloqueId == id);
            CrucigramaResultado resultado = new CrucigramaResultado();
    

            resultado.Correctas = new bool[12][];
            for (int i = 0; i < 12; i++)
            {
                resultado.Correctas[i] = new bool[12];
                for (int j = 0; j < 12; j++)
                {
                    var casilla = crucigrama.CasillaCrucigramas.FirstOrDefault(cr => cr.PosH == j && cr.PosV == i);
                    if(respuestas[i][j].ToUpper() == casilla.letra.ToUpper())
                    {
                        resultado.Correctas[i][j] = true;
                    }else
                    {
                        resultado.Correctas[i][j] = false;
                    }
                }
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
