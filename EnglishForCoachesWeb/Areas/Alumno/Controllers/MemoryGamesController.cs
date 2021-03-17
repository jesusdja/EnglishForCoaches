using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using EnglishForCoachesWeb.Database;
using EnglishForCoachesWeb.Database.JuegoOnline;
using EnglishForCoachesWeb.Areas.Alumno.Models;
using EnglishForCoachesWeb.Controllers;
using EnglishForCoachesWeb.Helpers;
using EnglishForCoachesWeb.Auth;
using System.Security.Claims;
using System.Collections.Generic;
using EnglishForCoachesWeb.DataAccess;

namespace EnglishForCoachesWeb.Areas.Alumno.Controllers
{
    [Authorize(Roles = "Alumno,Admin")]
    public class MemoryGamesController : MVCBaseController
    {
        private AuthContext db = new AuthContext();

        // GET: Admin/Contenidos
        public ActionResult Index(int id,bool? mistakes)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var juegoOnline = JuegoOnlineDataAccess.ObtenerJuegoOnlines(db).Include(bl => bl.SubTema.Tema).FirstOrDefault(b => b.JuegoOnlineId == id);

            if (juegoOnline == null)
                return HttpNotFound();

            if (!ComprobarAccesoSubTema(juegoOnline.SubTemaId))
                return RedirectToAction("SinAcceso", "Home", new { Area = "Alumno" });

     

            bool noMostrarMistakes = JuegoOnlineHelper.MarcarJuegoOnlineHecho(id);
            
            List<MemoryGame> MemoryGames = new List<MemoryGame>();
           
                MemoryGames = db.MemoryGames.Where(sk => sk.JuegoOnlineId == id).ToList();

            MemoryGameIndexViewModel viewModel = new MemoryGameIndexViewModel();
            
            var Imagenes = MemoryGames.Select(mtw =>new CasillaMemoryGame() { Dato= mtw.UrlImagen,Tipo="Imagen",Pareja= mtw.PalabraImagen }).ToList();
          
            var Palabras = MemoryGames.Select(mtw => new CasillaMemoryGame() { Dato = mtw.PalabraImagen, Tipo = "Palabra", Pareja = mtw.UrlImagen } ).ToList();
            Imagenes.AddRange(Palabras);
            viewModel.Casillas = Imagenes;
            viewModel.Casillas.Shuffle();

            viewModel.juegoOnline = juegoOnline;
            viewModel.MemoryGames = MemoryGames;
            

            return View(viewModel);
        }

      

        public JsonResult Contestar(int id, int respuesta)
        {
            MemoryGame MemoryGame = db.MemoryGames.Find(id);
            ResultadoMemoryGame resultado = new ResultadoMemoryGame();
          

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

