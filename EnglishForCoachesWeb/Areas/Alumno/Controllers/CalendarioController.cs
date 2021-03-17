using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

using EnglishForCoachesWeb.Areas.Alumno.Models;
using EnglishForCoachesWeb.Auth;
using EnglishForCoachesWeb.Controllers;
using EnglishForCoachesWeb.Database;
using EnglishForCoachesWeb.Database.Ejercicios;
using EnglishForCoachesWeb.Helpers;
using System;
using System.Security.Claims;
using System.Collections.Generic;
using System.Globalization;

namespace EnglishForCoachesWeb.Areas.Alumno.Controllers
{
    [Authorize(Roles = "Alumno,Admin")]
    public class CalendarioController : MVCBaseController
    {
        private AuthContext db = new AuthContext();

        
        // GET: Usuario/Home
        public ActionResult Index()
        {

            CalendarioIndexViewModel viewModel = new CalendarioIndexViewModel();
          

            return View(viewModel);
        }


        public JsonResult GetEvents(DateTime start,DateTime end)
        {
            List<EventoCalendario> listaEventos = new List<EventoCalendario>();
            var userId = ((ClaimsIdentity)HttpContext.User.Identity).FindFirst("UserId").Value;

            DateTime fechaDesde = start;
            DateTime fechaHasta = end;

            List<string> fechas = new List<string>();

            //acceso al sistema, ejercicios, exámenes, desbloqueo de extras, foro, glosario.
            var accesos=db.AccesoUsuarios.Where(AU=> AU.FechaAcceso >= fechaDesde && AU.FechaAcceso< fechaHasta && AU.UsuarioId== userId);
            foreach (var acceso in accesos) {
                string fecha = acceso.FechaAcceso.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                if (!fechas.Contains(fecha))
                {
                    fechas.Add(fecha);
                }
            }

            var ejercicios = db.BloqueRealizados.Where(ej => ej.FechaRealizado >= fechaDesde && ej.FechaRealizado < fechaHasta && ej.AlumnoId == userId);
            foreach (var ejercicio in ejercicios)
            {
                string fecha = ejercicio.FechaRealizado.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                if (!fechas.Contains(fecha))
                {
                    fechas.Add(fecha);
                }
            }

            var examenes = db.Examenes.Where(ej => ej.FechaExamen >= fechaDesde && ej.FechaExamen < fechaHasta && ej.AlumnoId == userId);
            foreach (var examen in examenes)
            {
                string fecha = examen.FechaExamen.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                if (!fechas.Contains(fecha))
                {
                    fechas.Add(fecha);
                }
            }

            var extraDesbloqueados = db.ExtraDesbloqueados.Where(ej => ej.FechaDesbloqueo >= fechaDesde && ej.FechaDesbloqueo < fechaHasta && ej.AlumnoId == userId);
            foreach (var extraDesbloqueado in extraDesbloqueados)
            {
                string fecha = extraDesbloqueado.FechaDesbloqueo.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                if (!fechas.Contains(fecha))
                {
                    fechas.Add(fecha);
                }
            }


            var foroMensajes = db.ForoMensajes.Where(ej => ej.FechaCreacion >= fechaDesde && ej.FechaCreacion < fechaHasta && ej.AlumnoId == userId);
            foreach (var foroMensaje in foroMensajes)
            {
                string fecha = foroMensaje.FechaCreacion.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                if (!fechas.Contains(fecha))
                {
                    fechas.Add(fecha);
                }
            }

            var vocabularioGlosarios = db.VocabularioGlosarios.Where(ej => ej.Fecha >= fechaDesde && ej.Fecha < fechaHasta && ej.AlumnoId == userId);
            foreach (var vocabularioGlosario in vocabularioGlosarios)
            {
                string fecha = vocabularioGlosario.Fecha.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                if (!fechas.Contains(fecha))
                {
                    fechas.Add(fecha);
                }
            }

            foreach (var fecha in fechas)
            {
                EventoCalendario evento = new EventoCalendario
                {
                    title = "Actividad",
                    description = "",
                    type = "meeting",
                    bg = "#039BE5",
                    allday = "true",
                    start = fecha,
                    end = fecha
                };
                listaEventos.Add(evento);
            }

            return Json(listaEventos, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEventsDay(string fecha)
        {
            DateTime day = Convert.ToDateTime(fecha);

            List<EventoDia> listaEventos = new List<EventoDia>();
            var userId = ((ClaimsIdentity)HttpContext.User.Identity).FindFirst("UserId").Value;

            

            //acceso al sistema, ejercicios, exámenes, desbloqueo de extras, foro, glosario.
            var accesos = db.AccesoUsuarios.Where(AU => DbFunctions.TruncateTime(AU.FechaAcceso) == day.Date && AU.UsuarioId == userId);
            foreach (var acceso in accesos)
            {
                EventoDia evento = new EventoDia
                {
                    texto = "<strong>Acceso</strong> a la plataforma",
                    tipo = "AccesoUsuario",
                    hora=acceso.FechaAcceso.ToShortTimeString()
                };
                listaEventos.Add(evento);
            }

            var ejercicios = db.BloqueRealizados.Where(ej => DbFunctions.TruncateTime(ej.FechaRealizado) == day.Date && ej.AlumnoId == userId).OrderBy(ej=>ej.FechaRealizado);
            if (ejercicios.Count() > 0)
            {
                string texto = "Se han realizado " + ejercicios.Count() + " <strong>ejercicios</strong>";
                if(ejercicios.Count()==1)
                {
                    texto = "Se ha realizado un <strong>ejercicio</strong>";
                }
                EventoDia eventoEjercicios = new EventoDia
                {
                    texto = texto,
                    tipo = "BloqueRealizado",
                    hora = ejercicios.First().FechaRealizado.ToShortTimeString()
                };
                listaEventos.Add(eventoEjercicios);

            }

            var examenes = db.Examenes.Where(ej => DbFunctions.TruncateTime(ej.FechaExamen) == day.Date && ej.AlumnoId == userId);
            foreach (var examen in examenes)
            {
                EventoDia evento = new EventoDia
                {
                    texto = "Se ha completado un <strong>examen</strong>",
                    tipo = "Examen",
                    hora = examen.FechaExamen.ToShortTimeString()
                };
                listaEventos.Add(evento);
            }

            var extraDesbloqueados = db.ExtraDesbloqueados.Where(ej => DbFunctions.TruncateTime(ej.FechaDesbloqueo) == day.Date && ej.AlumnoId == userId);
            foreach (var extraDesbloqueado in extraDesbloqueados)
            {
                EventoDia evento = new EventoDia
                {
                    texto = "Se ha desbloqueado un <strong>extra</strong>",
                    tipo = "ExtraDesbloqueado",
                    hora = extraDesbloqueado.FechaDesbloqueo.ToShortTimeString()
                };
                listaEventos.Add(evento);
            }


            var foroMensajes = db.ForoMensajes.Where(ej => DbFunctions.TruncateTime(ej.FechaCreacion) == day.Date && ej.AlumnoId == userId);
            foreach (var foroMensaje in foroMensajes)
            {
                EventoDia evento = new EventoDia
                {
                    texto = "Participación en <strong>foros</strong>",
                    tipo = "ForoMensaje",
                    hora = foroMensaje.FechaCreacion.ToShortTimeString()
                };
                listaEventos.Add(evento);
            }

            var vocabularioGlosarios = db.VocabularioGlosarios.Where(ej => DbFunctions.TruncateTime(ej.Fecha) == day.Date && ej.AlumnoId == userId);
            foreach (var vocabularioGlosario in vocabularioGlosarios)
            {
                EventoDia evento = new EventoDia
                {
                    texto = "Se ha añadido una palabra al <strong>glosario</strong>",
                    tipo = "VocabularioGlosario",
                    hora = vocabularioGlosario.Fecha.ToShortTimeString()
                };
                listaEventos.Add(evento);
            }

            

            return Json(listaEventos.OrderBy(le=>le.hora.PadLeft(5,'0')), JsonRequestBehavior.AllowGet);
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
