using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using EnglishForCoachesWeb.Validation;
using System.ComponentModel.DataAnnotations;
using EnglishForCoachesWeb.Database.Clientes;
using EnglishForCoachesWeb.Database.Varios;

namespace EnglishForCoachesWeb.Database
{
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "Nombre")]
        [RequiredLocalized]
        public string Nombre { get; set; }

        [Display(Name = "Apellido1")]
        [RequiredLocalized]
        public string Apellido1 { get; set; }

        [Display(Name = "Apellido2")]
        [RequiredLocalized]
        public string Apellido2 { get; set; }

        [Display(Name = "Grupo")]
        public Nullable<int> GrupoUsuarioId { get; set; }
        public virtual GrupoUsuario GrupoUsuario { get; set; }

        [Display(Name = "Cliente")]
        public Nullable<int> ClienteId { get; set; }
        public virtual Cliente Cliente { get; set; }

        [Display(Name = "Tipo")]
        public Nullable<int> TipoUsuarioId { get; set; }
        public virtual TipoUsuario TipoUsuario { get; set; }

        [Display(Name = "Puntos totales")]
        public int PuntosTotal { get; set; }
        [Display(Name = "Puntos actuales")]
        public int PuntosActual { get; set; }

        public bool BloquearSubtemas { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Tenga en cuenta que el valor de authenticationType debe coincidir con el definido en CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Agregar aquí notificaciones personalizadas de usuario
            if(userIdentity.Name!=null)
            {
                if (!manager.IsInRole(Id, "Admin"))
                {
                    AccesoUsuario acceso = new AccesoUsuario();
                    acceso.FechaAcceso = DateTime.Now;
                    acceso.Ip = HttpContext.Current == null ? "" : HttpContext.Current.Request.UserHostAddress;
                    acceso.Usuario = Nombre + " " + Apellido1 + " " + Apellido2;
                    acceso.UsuarioId = Id;

                    AuthContext db = new AuthContext();
                    db.AccesoUsuarios.Add(acceso);
                    db.SaveChanges();
                }
            }

            userIdentity.AddClaim(new Claim("BloquearSubtemas", BloquearSubtemas.ToString()));
            userIdentity.AddClaim(new Claim("UserId", Id));
            userIdentity.AddClaim(new Claim("NombreUsuario", Nombre + " " + Apellido1 + " " + Apellido2));
            //Avatar claim            
            userIdentity.AddClaim(new Claim("avatar", "/media/upload/avatar/" + Id + ".jpg"));
           
            if (manager.IsInRole(Id, "Admin"))
            {
                userIdentity.AddClaim(new Claim("GrupoUsuario", ""));
                userIdentity.AddClaim(new Claim("ClienteId", ""));
                userIdentity.AddClaim(new Claim("ClienteUrl", ""));
            }
            else
            {
                AuthContext db = new AuthContext();
                var cliente = db.Clientes.Find(ClienteId);
                userIdentity.AddClaim(new Claim("GrupoUsuario", GrupoUsuarioId.Value.ToString()));
                userIdentity.AddClaim(new Claim("ClienteId", ClienteId.GetValueOrDefault().ToString()));
                if (cliente.NombreUrl == null)
                    userIdentity.AddClaim(new Claim("ClienteUrl", ""));
                else
                    userIdentity.AddClaim(new Claim("ClienteUrl", cliente.NombreUrl));
            }
            return userIdentity;
        }
    }
    

    public class AuthContext : IdentityDbContext<ApplicationUser>
    {        
        public AuthContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }
    

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public static AuthContext Create()
        {
            return new AuthContext();
        }
        public System.Data.Entity.DbSet<EnglishForCoachesWeb.Database.GrupoUsuario> GrupoUsuarios { get; set; }

        public System.Data.Entity.DbSet<EnglishForCoachesWeb.Database.Articulo> Articulos { get; set; }

        public System.Data.Entity.DbSet<EnglishForCoachesWeb.Database.Categoria> Categorias { get; set; }

        public System.Data.Entity.DbSet<EnglishForCoachesWeb.Database.Tag> Tags { get; set; }

        public System.Data.Entity.DbSet<EnglishForCoachesWeb.Database.Comentario> Comentarios { get; set; }


        public System.Data.Entity.DbSet<EnglishForCoachesWeb.Database.Ejercicios.TipoEjercicio> TipoEjercicios { get; set; }

        public System.Data.Entity.DbSet<EnglishForCoachesWeb.Database.Ejercicios.Tema> Temas { get; set; }

        public System.Data.Entity.DbSet<EnglishForCoachesWeb.Database.Ejercicios.SubTema> SubTemas { get; set; }

        public System.Data.Entity.DbSet<EnglishForCoachesWeb.Database.Ejercicios.Area> Areas { get; set; }

        public System.Data.Entity.DbSet<EnglishForCoachesWeb.Database.Ejercicios.Test> Tests { get; set; }

        public System.Data.Entity.DbSet<EnglishForCoachesWeb.Database.Ejercicios.Skillwise> Skillwises { get; set; }

        public System.Data.Entity.DbSet<EnglishForCoachesWeb.Database.Ejercicios.MatchTheWord> MatchTheWords { get; set; }

        public System.Data.Entity.DbSet<EnglishForCoachesWeb.Database.Ejercicios.FillTheGap> FillTheGaps { get; set; }

        public System.Data.Entity.DbSet<EnglishForCoachesWeb.Database.Ejercicios.WordByWord> WordByWords { get; set; }
        public System.Data.Entity.DbSet<EnglishForCoachesWeb.Database.Ejercicios.Bloque> Bloques { get; set; }
        public System.Data.Entity.DbSet<EnglishForCoachesWeb.Database.Ejercicios.Gramatica> Gramaticas { get; set; }
        public System.Data.Entity.DbSet<EnglishForCoachesWeb.Database.Ejercicios.SubTemaDesbloqueado> SubTemaDesbloqueados { get; set; }
        public System.Data.Entity.DbSet<EnglishForCoachesWeb.Database.Ejercicios.AudioSentenceExercise> AudioSentenceExercises { get; set; }
        public System.Data.Entity.DbSet<EnglishForCoachesWeb.Database.Ejercicios.Vocabulario> Vocabularios { get; set; }

        public System.Data.Entity.DbSet<EnglishForCoachesWeb.Database.Varios.AccesoUsuario> AccesoUsuarios { get; set; }
        public System.Data.Entity.DbSet<EnglishForCoachesWeb.Database.Ejercicios.Frase> Frases { get; set; }

        public System.Data.Entity.DbSet<EnglishForCoachesWeb.Database.Ejercicios.Examen> Examenes { get; set; }

        public System.Data.Entity.DbSet<EnglishForCoachesWeb.Database.Ejercicios.BloqueRealizado> BloqueRealizados { get; set; }

        public System.Data.Entity.DbSet<EnglishForCoachesWeb.Database.Ejercicios.VocabularioGlosario> VocabularioGlosarios { get; set; }
        public System.Data.Entity.DbSet<EnglishForCoachesWeb.Database.Ejercicios.FraseGlosario> FraseGlosarios { get; set; }
        

        public System.Data.Entity.DbSet<EnglishForCoachesWeb.Database.Ejercicios.CategoriaExtra> CategoriaExtras { get; set; }
        public System.Data.Entity.DbSet<EnglishForCoachesWeb.Database.Ejercicios.Extra> Extras { get; set; }

        public System.Data.Entity.DbSet<EnglishForCoachesWeb.Database.Ejercicios.ExtraDesbloqueado> ExtraDesbloqueados { get; set; }


        public System.Data.Entity.DbSet<EnglishForCoachesWeb.Database.Foro.ForoCategoria> ForoCategorias { get; set; }
        public System.Data.Entity.DbSet<EnglishForCoachesWeb.Database.Foro.ForoHilo> ForoHilos { get; set; }
        public System.Data.Entity.DbSet<EnglishForCoachesWeb.Database.Foro.ForoMensaje> ForoMensajes { get; set; }

        public System.Data.Entity.DbSet<EnglishForCoachesWeb.Database.Foro.ForoHiloLeido> ForoHiloLeidos { get; set; }

        public System.Data.Entity.DbSet<EnglishForCoachesWeb.Database.Ejercicios.CategoriaJuego> CategoriaJuegos { get; set; }
        public System.Data.Entity.DbSet<EnglishForCoachesWeb.Database.Ejercicios.Juego> Juegos { get; set; }

        public System.Data.Entity.DbSet<EnglishForCoachesWeb.Database.Varios.Noticia> Noticias { get; set; }
        public System.Data.Entity.DbSet<EnglishForCoachesWeb.Database.Varios.NoticiaGrupo> NoticiaGrupos { get; set; }
        public System.Data.Entity.DbSet<EnglishForCoachesWeb.Database.Ejercicios.CategoriaVocabulario> CategoriaVocabularios { get; set; }
        public System.Data.Entity.DbSet<EnglishForCoachesWeb.Database.Ejercicios.Documento> Documentos { get; set; }
        public System.Data.Entity.DbSet<EnglishForCoachesWeb.Database.Ejercicios.DocumentoGrupo> DocumentoGrupos { get; set; }
        public System.Data.Entity.DbSet<EnglishForCoachesWeb.Database.Ejercicios.Crucigrama> Crucigramas { get; set; }

        public System.Data.Entity.DbSet<EnglishForCoachesWeb.Database.Ejercicios.CasillaCrucigrama> CasillaCrucigramas { get; set; }

        public System.Data.Entity.DbSet<EnglishForCoachesWeb.Database.Ejercicios.BloqueDesbloqueado> BloqueDesbloqueados { get; set; }

        public System.Data.Entity.DbSet<EnglishForCoachesWeb.Database.Ejercicios.Mistake> Mistakes { get; set; }
        public System.Data.Entity.DbSet<EnglishForCoachesWeb.Database.Ejercicios.ExamenDesbloqueado> ExamenDesbloqueados { get; set; }
        public System.Data.Entity.DbSet<EnglishForCoachesWeb.Database.Ejercicios.MatchThePicture> MatchThePictures { get; set; }
        public System.Data.Entity.DbSet<EnglishForCoachesWeb.Database.Ejercicios.SubTemaAccesoUsuario> SubTemaAccesoUsuarios { get; set; }
        public System.Data.Entity.DbSet<EnglishForCoachesWeb.Database.Ejercicios.TrueFalse> TrueFalses { get; set; }
        public System.Data.Entity.DbSet<EnglishForCoachesWeb.Database.Varios.Video> Videos { get; set; }
        public System.Data.Entity.DbSet<EnglishForCoachesWeb.Database.Ejercicios.SubTemaVideo> SubTemaVideos { get; set; }
        public System.Data.Entity.DbSet<EnglishForCoachesWeb.Database.JuegoOnline.BeatTheGoalie> BeatTheGoalies { get; set; }
        public System.Data.Entity.DbSet<EnglishForCoachesWeb.Database.JuegoOnline.TipoJuegoOnline> TipoJuegoOnlines { get; set; }
        public System.Data.Entity.DbSet<EnglishForCoachesWeb.Database.JuegoOnline.JuegoOnline> JuegoOnlines { get; set; }
        public System.Data.Entity.DbSet<EnglishForCoachesWeb.Database.JuegoOnline.JuegoOnlineRealizado> JuegoOnlineRealizados { get; set; }
        public System.Data.Entity.DbSet<EnglishForCoachesWeb.Database.JuegoOnline.JuegoOnlineDesbloqueado> JuegoOnlineDesbloqueados { get; set; }
        public System.Data.Entity.DbSet<EnglishForCoachesWeb.Database.JuegoOnline.MemoryGame> MemoryGames { get; set; }

        public System.Data.Entity.DbSet<EnglishForCoachesWeb.Database.TipoUsuario> TipoUsuarios { get; set; }

        public System.Data.Entity.DbSet<EnglishForCoachesWeb.Database.Ejercicios.FillTheBox> FillTheBoxs { get; set; }
        
        public System.Data.Entity.DbSet<EnglishForCoachesWeb.Database.Ejercicios.DefinicionVocabulario> DefinicionVocabularios { get; set; }

        public System.Data.Entity.DbSet<EnglishForCoachesWeb.Database.JuegoOnline.SopaLetras> SopaLetras { get; set; }
        public System.Data.Entity.DbSet<EnglishForCoachesWeb.Database.JuegoOnline.CasillaSopaLetras> CasillaSopaLetras { get; set; }
        public System.Data.Entity.DbSet<EnglishForCoachesWeb.Database.JuegoOnline.ComentarioSopaLetras> ComentarioSopaLetras { get; set; }
        public System.Data.Entity.DbSet<EnglishForCoachesWeb.Database.JuegoOnline.VocabularioSopaLetras> VocabularioSopaLetras { get; set; }


        public System.Data.Entity.DbSet<EnglishForCoachesWeb.Database.JuegoOnline.Ahorcado> Ahorcado { get; set; }


        public System.Data.Entity.DbSet<EnglishForCoachesWeb.Database.Ejercicios.GramaticaCuerpo> GramaticaCuerpo { get; set; }
        public System.Data.Entity.DbSet<EnglishForCoachesWeb.Database.Ejercicios.ExtraExplicacion> ExtraExplicacion { get; set; }

        //clientes
        public System.Data.Entity.DbSet<Cliente> Clientes { get; set; }
        public System.Data.Entity.DbSet<ClienteBloque> ClienteBloques { get; set; }
        public System.Data.Entity.DbSet<ClienteExtra> ClienteExtras { get; set; }
        public System.Data.Entity.DbSet<ClienteGramatica> ClienteGramaticas { get; set; }
        public System.Data.Entity.DbSet<ClienteJuego> ClienteJuegos { get; set; }
        public System.Data.Entity.DbSet<ClienteJuegoOnline> ClienteJuegoOnlines { get; set; }
        public System.Data.Entity.DbSet<ClienteSubTema> ClienteSubTemas { get; set; }
        public System.Data.Entity.DbSet<ClienteTema> ClienteTemas { get; set; }
        public System.Data.Entity.DbSet<ClienteSubTemaVideo> ClienteSubTemaVideos { get; set; }
    }

}