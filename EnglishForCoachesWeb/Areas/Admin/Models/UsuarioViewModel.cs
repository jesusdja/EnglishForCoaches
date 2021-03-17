using EnglishForCoachesWeb.DataAccess;
using EnglishForCoachesWeb.Database;
using EnglishForCoachesWeb.Database.Ejercicios;
using EnglishForCoachesWeb.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnglishForCoachesWeb.Areas.Admin.Models
{

    public class UsuarioIndexViewModel
    {
        public string TextoBusqueda { get; set; }
        public List<ApplicationUser> Usuarios { get; set; }
    }

    public class UsuarioGruposViewModel
    {
        public List<GrupoUsuario> Grupos { get; set; }
    }

    public class UsuarioCreateGrupoViewModel
    {
        public GrupoUsuario GrupoUsuario { get; set; }
    }

    public  class UsuarioEditGrupoViewModel
    {
        public GrupoUsuario GrupoUsuario { get; set; }
    }
        public  class AccesoTema
    {
        public Tema Tema { get; set; }
        public List<SelectListItem> SubTemas { get; set; }
    }

        public abstract class UsuarioViewModel
    {

        public ApplicationUser Usuario { get; set; }

        public List<AccesoTema> AccesoTemas { get; set; }

        public SelectList GrupoUsuarioSelectList { get; set; }
        public SelectList TipoUsuarioSelectList { get; set; }
        public SelectList ClienteSelectList { get; set; }
        public void InicializarDesplegables()
        {
            AuthContext db = new AuthContext();

            GrupoUsuarioSelectList = new SelectList(db.GrupoUsuarios, "GrupoUsuarioId", "Descripcion");
            TipoUsuarioSelectList = new SelectList(db.TipoUsuarios, "TipoUsuarioId", "Descripcion");
            ClienteSelectList = new SelectList(db.Clientes, "ClienteId", "Descripcion");
            CargarAccesoSeleccionado(db);
        }

        protected abstract void CargarAccesoSeleccionado(AuthContext db);
    }

    public class UsuarioCreateViewModel : UsuarioViewModel
    {
        [Required]
        public string Password { get; set; }
        protected override void CargarAccesoSeleccionado(AuthContext db)
        {
            
                var temas = db.Temas.ToList();
                AccesoTemas = new List<AccesoTema>();
                foreach (Tema tema in temas)
                {
                    var accesoTema = new AccesoTema
                    {
                        Tema = tema,
                        SubTemas = db.SubTemas.Where(st => st.TemaId == tema.TemaId).OrderBy(st => st.Orden).Select(subt => new SelectListItem { Text = subt.Descripcion, Value = subt.SubTemaId.ToString() }).ToList()
                    };
                    AccesoTemas.Add(accesoTema);
                }

            
        }
    }

    public class UsuarioEditViewModel : UsuarioViewModel
    {
        
        public string Password { get; set; }



        protected override void CargarAccesoSeleccionado(AuthContext db)
        {
                var temas = TemaDataAccess.ObtenerTemasCliente(db, Usuario.ClienteId).ToList();
                AccesoTemas = new List<AccesoTema>();

                var Accesos = db.SubTemaAccesoUsuarios.Where(sau => sau.AlumnoId == Usuario.Id).Select(sau=>sau.SubTemaId).ToList();

                foreach (Tema tema in temas)
                {
                    var accesoTema = new AccesoTema
                    {
                        Tema = tema,
                        SubTemas = SubTemaDataAccess.ObtenerSubTemasCliente(db, Usuario.ClienteId).Where(st => st.TemaId == tema.TemaId).OrderBy(st => st.Orden).Select(subt => new SelectListItem { Text = subt.Descripcion, Value = subt.SubTemaId.ToString(),Selected= Accesos.Contains(subt.SubTemaId) }).ToList()
                    };
                    AccesoTemas.Add(accesoTema);
                }                            
        }
    }
}