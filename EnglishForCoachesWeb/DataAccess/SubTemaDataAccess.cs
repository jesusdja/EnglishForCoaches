using EnglishForCoachesWeb.Database;
using EnglishForCoachesWeb.Database.Clientes;
using EnglishForCoachesWeb.Database.Ejercicios;
using EnglishForCoachesWeb.Database.Foro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace EnglishForCoachesWeb.DataAccess
{
    public static class SubTemaDataAccess
    {
        public static IQueryable<SubTema> ObtenerSubTemas(AuthContext db )
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
                    var ClienteActual = db.Clientes.FirstOrDefault(cli=>cli.NombreUrl== clienteURL);
                    if (ClienteActual != null)
                    {
                        clienteFiltrar = ClienteActual.ClienteId;
                    }
                }
            }
            var clienteSubTemas= db.ClienteSubTemas.Where(cli => cli.ClienteId== clienteFiltrar).Select(cli=>cli.SubTemaId).ToList();
            return db.SubTemas.Where(fh=> clienteSubTemas.Contains(fh.SubTemaId));
        }

        public static IQueryable<SubTema> ObtenerSubTemasCliente(AuthContext db,int? clienteId)
        {
            int clienteFiltrar = (int)ClientesId.Azulita;
            if (clienteId.HasValue)
            {
                clienteFiltrar = clienteId.Value;
            }
            var clienteSubTemas = db.ClienteSubTemas.Where(cli => cli.ClienteId == clienteFiltrar).Select(cli => cli.SubTemaId).ToList();
            return db.SubTemas.Where(fh => clienteSubTemas.Contains(fh.SubTemaId));
        }
    }
}