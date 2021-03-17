using EnglishForCoachesWeb.Database;
using EnglishForCoachesWeb.Database.Clientes;
using EnglishForCoachesWeb.Database.Ejercicios;
using EnglishForCoachesWeb.Database.Foro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace EnglishForCoachesWeb.Helpers
{
    public static class ForoHiloDataAccess
    {
        public static List<ForoHilo> ObtenerForoHilo( )
        {
            AuthContext db = new AuthContext();



            var ClienteId = ((ClaimsIdentity)HttpContext.Current.User.Identity).FindFirst("ClienteId").Value;
            if (string.IsNullOrEmpty(ClienteId))
            {
                ClienteId = ((int)ClientesId.Azulita).ToString();
            }
            string cliente = (string)HttpContext.Current.Request.RequestContext.RouteData.Values["cliente"];
            var clienteActual = db.Clientes.FirstOrDefault(cl => cl.NombreUrl.ToUpper() == cliente.ToUpper());
            if (clienteActual == null)
            {
                clienteActual = db.Clientes.Find((int)ClientesId.Azulita);
            }
            ClienteId = clienteActual.ClienteId.ToString();
            int clienteIdFiltrar = Convert.ToInt32(ClienteId);
            if (HttpContext.Current.User.IsInRole("Admin"))
            {
                return db.ForoHilos.Where(fo=>fo.ClienteId== clienteIdFiltrar).ToList();
            }

            var userId = ((ClaimsIdentity)HttpContext.Current.User.Identity).FindFirst("UserId").Value;
            int GrupoUsuarioId = Convert.ToInt32(((ClaimsIdentity)HttpContext.Current.User.Identity).FindFirst("GrupoUsuario").Value);
            var usuariosIds=db.Users.Where(us=>us.GrupoUsuarioId.Value== GrupoUsuarioId).Select(us=>us.Id).ToList();

            return db.ForoHilos.Where(fh=> fh.ClienteId == clienteIdFiltrar && usuariosIds.Contains(fh.AlumnoId)).OrderByDescending(fh=>fh.FechaRespuesta).ToList();

        }
     
    }
}