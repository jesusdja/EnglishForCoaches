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
    public static class ExtraDataAccess
    {
        public static IQueryable<Extra> ObtenerExtras(AuthContext db )
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
            var clienteExtras= db.ClienteExtras.Where(cli => cli.ClienteId== clienteFiltrar).Select(cli=>cli.ExtraId).ToList();
            return db.Extras.Where(fh=> clienteExtras.Contains(fh.ExtraId));
        }
     
    }
}