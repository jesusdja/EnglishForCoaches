using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

using EnglishForCoachesWeb.Areas.Alumno.Models;
using EnglishForCoachesWeb.Auth;
using EnglishForCoachesWeb.Controllers;
using EnglishForCoachesWeb.Database;
using EnglishForCoachesWeb.Database.Ejercicios;
using EnglishForCoachesWeb.Database.Clientes;
using EnglishForCoachesWeb.Helpers;
using System;
using System.Security.Claims;
using EnglishForCoachesWeb.DataAccess;
using System.Collections.Generic;

namespace EnglishForCoachesWeb.Areas.Alumno.Controllers
{
    //css
    public class CssViewResult : PartialViewResult
    {
        public CssViewResult()
        {

        }

        public CssViewResult(object model)
        {
            if (model != null)
            {
                ViewData.Model = model;
            }
        }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.ContentType = "text/css";
            base.ExecuteResult(context);
        }
    }


    [Authorize(Roles = "Alumno,Admin")]
    public class HomeController : MVCBaseController
    {
        private AuthContext db = new AuthContext();
               
        // GET: Usuario/Home
        [AllowAnonymous]
        public ActionResult CssCliente()
        {
            string cliente=(string)RouteData.Values["cliente"];
            var clienteActual = db.Clientes.FirstOrDefault(cl=>cl.NombreUrl.ToUpper()== cliente.ToUpper());
            if(clienteActual==null)
            {
                clienteActual = db.Clientes.Find((int)ClientesId.Azulita);
            }
            return new CssViewResult(clienteActual);
        }

        [ChildActionOnly]
        [AllowAnonymous]
        public ActionResult LogoLogin()
        {

            var imgSrc = "~/Content/Alumno/images/logo-login.png";
            string cliente = (string)RouteData.Values["cliente"];
            var clienteActual = db.Clientes.FirstOrDefault(cl => cl.NombreUrl.ToUpper() == cliente.ToUpper());
            if (clienteActual == null)
            {
                clienteActual = db.Clientes.Find((int)ClientesId.Azulita);
            }
            if (clienteActual != null)
            {
                imgSrc = "~/media/upload/Cliente/" + clienteActual.ClienteId + "_logoNegro.jpg";
            }
            return PartialView("_LogoLogin", imgSrc);
        }

        [ChildActionOnly]
        public ActionResult Logo()
        {

            var imgSrc = "~/Content/Alumno/images/logo_alumno.png";
            string cliente = (string)RouteData.Values["cliente"];
            var clienteActual = db.Clientes.FirstOrDefault(cl => cl.NombreUrl.ToUpper() == cliente.ToUpper());
            if (clienteActual == null)
            {
                clienteActual = db.Clientes.Find((int)ClientesId.Azulita);
            }
            if (clienteActual != null)
            {
                imgSrc = "~/media/upload/Cliente/" + clienteActual.ClienteId + "_logoBlanco.jpg";
            }
            return PartialView("_Logo", imgSrc);
        }

        // GET: Usuario/Home
        public ActionResult Index()
        {
            var temas = TemaDataAccess.ObtenerTemas(db).ToList();
            var temasHome= new List<TemaHome>();

            AuthRepository authRepository = new AuthRepository();
            ApplicationUser user = authRepository.FindByName(User.Identity.Name);

            foreach (var tema in temas)
            {
                var subtemas = SubTemaDataAccess.ObtenerSubTemas(db).Where(st => st.TemaId == tema.TemaId).Select(st => st.SubTemaId);

                var subtemasAprobados = db.Examenes.Where(e => e.AlumnoId == user.Id && e.Aprobado &&
             subtemas.Contains(e.SubTemaId)).Select(e => e.SubTemaId).Distinct().Count();

                double porcentaje = (double)subtemasAprobados / subtemas.Count() * 100;

                temasHome.Add(new TemaHome()
                {
                    Porcentaje = (int) porcentaje,
                    Tema = tema
                });
            }

         

            var _examenes = db.Examenes.Include(e => e.SubTema).Include(e => e.SubTema.Tema).Where(e => e.AlumnoId == user.Id).OrderByDescending(e => e.FechaExamen).Take(3).ToList();


            HomeIndexViewModel viewModel = new HomeIndexViewModel();        

            viewModel.temasHome = temasHome;



            viewModel.NRealizados = db.BloqueRealizados.Count(br=>br.AlumnoId==user.Id);
            viewModel.NExtras = db.ExtraDesbloqueados.Count(br => br.AlumnoId == user.Id);


            var GrupoUsuarioID = ((ClaimsIdentity)User.Identity).FindFirst("GrupoUsuario").Value;
            if (GrupoUsuarioID == "")
            {
                viewModel.Noticias = NoticiaDataAccess.ObtenerNoticia().OrderByDescending(not => not.Fecha).Take(3).ToList();
            }
            else
            {
                viewModel.Noticias = db.NoticiaGrupos.Where(ng => ng.GrupoUsuarioId.ToString() == GrupoUsuarioID)
                    .Select(gu => gu.Noticia).OrderByDescending(not => not.Fecha).Take(3).ToList();
            }

            viewModel.Hilos= ForoHiloDataAccess.ObtenerForoHilo().Take(3).ToList();


            viewModel.Puntos = user.PuntosActual;

            viewModel.Examenes = _examenes;

            return View(viewModel);
        }

        // GET: Usuario/SinAcceso
        public ActionResult SinAcceso()
        {
            return View();
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