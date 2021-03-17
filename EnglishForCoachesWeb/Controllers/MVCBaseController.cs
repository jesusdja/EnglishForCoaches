using EnglishForCoachesWeb.Database;
using EnglishForCoachesWeb.Database.Clientes;
using EnglishForCoachesWeb.Database.Varios;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;

namespace EnglishForCoachesWeb.Controllers
{
    public class MVCBaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var culture = new CultureInfo("es-ES"); // Get the culture name from the route values / request querystring / form / cookie
            Thread.CurrentThread.CurrentUICulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;
            
            AuthContext db = new AuthContext();
            var accesos = db.AccesoUsuarios.Where(ac => ac.Ip == "" && ac.Usuario == User.Identity.Name).ToList();
            foreach (AccesoUsuario au in accesos)
            {
                au.Ip = filterContext.HttpContext.Request.UserHostAddress;
                db.Entry(au).State = EntityState.Modified;
                db.SaveChanges();
            }

            if (((ClaimsIdentity)User.Identity).FindFirst("ClienteUrl") != null)
            {
                var clienteUsuarioUrl = ((ClaimsIdentity)User.Identity).FindFirst("ClienteUrl").Value;
                if (!ComprobarAccesoCliente(clienteUsuarioUrl))
                {
                    if (!string.IsNullOrWhiteSpace(clienteUsuarioUrl))
                    {
                        filterContext.Result = RedirectToAction("Login", "Account", new { Area = "Alumno", cliente = clienteUsuarioUrl });
                    }
                    else
                    {
                        filterContext.Result = RedirectToAction("Login", "Account", new { Area = "Alumno" });
                    }
                    return;
                }
            }


            base.OnActionExecuting(filterContext);
        }

        private bool ComprobarAccesoCliente(string clienteUsuarioUrl)
        {

            if (User.IsInRole("Admin"))
            {
                return true;
            }
            string clienteurl = (string)RouteData.Values["cliente"];
            if (string.IsNullOrEmpty(clienteurl))
            {
                clienteurl = "";
            }
            return clienteUsuarioUrl == clienteurl;
        }

        public bool ComprobarAccesoSubTema(int subtemaId)
        {
            AuthContext db = new AuthContext();
            if (User.IsInRole("Admin"))
            {
                return true;
            }

                var userId = ((ClaimsIdentity)User.Identity).FindFirst("UserId").Value;
            var subtemaDesbloqueado = db.SubTemaDesbloqueados.FirstOrDefault(sd => sd.AlumnoId == userId && sd.SubTemaId == subtemaId);

            if (subtemaDesbloqueado == null)
            {
                return false;
            }
            return true;
        }

        public bool ComprobarAccesoExamen(int subtemaId)
        {
            bool response = false;
            if (User.IsInRole("Admin"))
            {
                return true;
            }
            AuthContext db = new AuthContext();
            var userId = ((ClaimsIdentity)User.Identity).FindFirst("UserId").Value;
            var subtema = db.SubTemas.FirstOrDefault(s => s.SubTemaId == subtemaId);

            if (subtema != null)
            {
                var maxsubtemaDesbloqueado = db.SubTemaDesbloqueados.Where(s => s.SubTema.TemaId == subtema.TemaId && s.AlumnoId == userId).OrderByDescending(s => s.SubTema.Orden).FirstOrDefault();
                if (maxsubtemaDesbloqueado != null)
                    if (maxsubtemaDesbloqueado.SubTemaId == subtemaId)
                        response = true;
            }



            return response;
        }
    }
}