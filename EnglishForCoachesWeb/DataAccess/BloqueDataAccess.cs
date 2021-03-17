using EnglishForCoachesWeb.Database;
using EnglishForCoachesWeb.Database.Clientes;
using EnglishForCoachesWeb.Database.Ejercicios;
using EnglishForCoachesWeb.Database.Foro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Routing;

namespace EnglishForCoachesWeb.DataAccess
{
    public static class BloqueDataAccess
    {
        public static IQueryable<Bloque> ObtenerBloques(AuthContext db )
        {
            var clienteId = ((ClaimsIdentity)HttpContext.Current.User.Identity).FindFirst("ClienteId").Value;
            int clienteFiltrar = (int)ClientesId.Azulita;
            if (!string.IsNullOrWhiteSpace(clienteId))
            {
                clienteFiltrar = Convert.ToInt32(clienteId);
            }
            else
            {
                var clienteURL = (string)HttpContext.Current.Request.RequestContext.RouteData.Values["cliente"];
                if (!string.IsNullOrWhiteSpace(clienteURL))
                {
                    var ClienteActual = db.Clientes.FirstOrDefault(cli => cli.NombreUrl == clienteURL);
                    if (ClienteActual != null)
                    {
                        clienteFiltrar = ClienteActual.ClienteId;
                    }
                }
            }
            var clienteBloques= db.ClienteBloques.Where(cli => cli.ClienteId== clienteFiltrar).Select(cli=>cli.BloqueId).ToList();
            return db.Bloques.Where(fh=> clienteBloques.Contains(fh.BloqueId));
        }
     
    }
}