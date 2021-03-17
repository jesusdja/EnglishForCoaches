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
    public class DocumentosController : MVCBaseController
    {
        private AuthContext db = new AuthContext();

        // GET: Alumno/Documentos
        public ActionResult Index()
        {
            DocumentosIndexViewModel viewModel = new DocumentosIndexViewModel();
            
            var GrupoUsuarioID = ((ClaimsIdentity)User.Identity).FindFirst("GrupoUsuario").Value;

            var userId = ((ClaimsIdentity)User.Identity).FindFirst("UserId").Value;

            var subtemasDesbloqueados = db.SubTemaDesbloqueados.Where(sd => sd.AlumnoId == userId
            ).Select(sd => sd.SubTemaId).ToList();
            if (GrupoUsuarioID == "")
            {
                viewModel.Documentos = db.Documentos.OrderByDescending(not => not.Fecha).ToList();
            }else
            {
                viewModel.Documentos = db.DocumentoGrupos.Where(ng=>ng.GrupoUsuarioId.ToString()==GrupoUsuarioID)
                    .Select(gu=>gu.Documento).OrderByDescending(not => not.Fecha).ToList();
            }
            viewModel.Documentos = viewModel.Documentos.ToList();
            List<int> idsTemas = viewModel.Documentos.Select(doc => doc.TemaId.HasValue? doc.TemaId .Value: -1).Distinct().ToList();
            List<int> idsSubTemas = viewModel.Documentos.Select(doc => doc.SubTemaId.HasValue ? doc.SubTemaId.Value : -1).Distinct().ToList();
            viewModel.Temas = TemaDataAccess.ObtenerTemas(db).Where(t=> idsTemas.Contains(t.TemaId)).ToList();
            if (User.IsInRole("Admin"))
            {
                viewModel.Subtemas = SubTemaDataAccess.ObtenerSubTemas(db).Where(t => idsSubTemas.Contains(t.SubTemaId)).ToList();
            }
            else
            {
                viewModel.Subtemas = SubTemaDataAccess.ObtenerSubTemas(db).Where(t => idsSubTemas.Contains(t.SubTemaId) && subtemasDesbloqueados.Contains(t.SubTemaId)).ToList();
            }

            return View(viewModel);
        }

        public FileResult Download(int id)
        {
            var documento = db.Documentos.FirstOrDefault(doc=>doc.DocumentoId==id);
            if (documento != null)
            {
                string nameAndLocation = "~/media/upload/documentos/" + documento.FicheroAdjunto;

                byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath(nameAndLocation));
                string fileName = documento.FicheroAdjunto;
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            return null;
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
