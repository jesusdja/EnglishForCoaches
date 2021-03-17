namespace EnglishForCoachesWeb.Migrations
{
    using Database;
    using Database.Ejercicios;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<EnglishForCoachesWeb.Database.AuthContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(EnglishForCoachesWeb.Database.AuthContext context)
        {
            //  This method will be called after migrating to the latest version.
            //if (!System.Diagnostics.Debugger.IsAttached)
            //    System.Diagnostics.Debugger.Launch();
            MoverCuerpos(context);

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //context.Nivel.AddOrUpdate(

            //context.Categorias.AddOrUpdate(
            //      p => p.Descripcion,
            //      new Categoria { Descripcion = "The football glossary", DescripcionSeo= "the-football-glossary" },
            //      new Categoria { Descripcion = "Idioms related to sport", DescripcionSeo = "idioms-related-to-sport" },
            //      new Categoria { Descripcion = "Teacher tips", DescripcionSeo = "teacher-tips" },
            //      new Categoria { Descripcion = "Podcasts", DescripcionSeo = "podcasts" },
            //      new Categoria { Descripcion = "The language of...", DescripcionSeo = "the-language-of" }
            //    );

            //context.Temas.AddOrUpdate(
            //      p => p.Descripcion,
            //      new Tema { Descripcion = "Gramática"},
            //      new Tema { Descripcion = "Unidades didácticas"},
            //      new Tema { Descripcion = "English for football" }
            //    );

            //context.Areas.AddOrUpdate(
            //      p => p.Descripcion,
            //      new Area { Descripcion = "Learning" },
            //      new Area { Descripcion = "Practising" },
            //      new Area { Descripcion = "Improving" },
            //      new Area { Descripcion = "Training" },
            //      new Area { Descripcion = "Listening" },
            //      new Area { Descripcion = "Answering" },
            //      new Area { Descripcion = "Playing" }
            //    );

            //context.TipoEjercicios.AddOrUpdate(
            //      p => p.Descripcion,
            //      new TipoEjercicio { Descripcion = "Test" },
            //      new TipoEjercicio { Descripcion = "Skillwise" },
            //      new TipoEjercicio { Descripcion = "Match the word" },
            //      new TipoEjercicio { Descripcion = "Fill the next" },
            //      new TipoEjercicio { Descripcion = "Word by word" }
            //    );


            context.GrupoUsuarios.AddOrUpdate(
                  p => p.Descripcion,
                  new GrupoUsuario { Descripcion = "Tecnicos RFEF" }
                );

            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            
                   //Create Role Admin if it does not exist
            string role = "AdministradorGrupo";
            if (!RoleManager.RoleExists(role))
            {
                var roleresult = RoleManager.Create(new IdentityRole(role));
            }

            //Create Role Admin if it does not exist
             role = "Admin";
            if (!RoleManager.RoleExists(role))
            {
                var roleresult = RoleManager.Create(new IdentityRole(role));
            }

            //Create User=Admin with password=adm456
            string name = "eneko@campusdeportivo.com";
            string password = "adm456";

            var user = new ApplicationUser();
            user.UserName = name;
            user.Email = name;
            //var userResult = UserManager.Create(user, password);

            ////Add User Admin to Role Admin
            //if (userResult.Succeeded)
            //{
            //    var result = UserManager.AddToRole(user.Id, role);
            //}
            //Create User=Admin with password=adm456
             name = "ibai@campusdeportivo.com";
             password = "adm456";

            user = new ApplicationUser();
            user.UserName = name;
            user.Email = name;
            //userResult = UserManager.Create(user, password);

            ////Add User Admin to Role Admin
            //if (userResult.Succeeded)
            //{
            //    var result = UserManager.AddToRole(user.Id, role);
            //}
            //Create User=Admin with password=adm456
            name = "silvia.moreno@rfef.es";
            password = "sil435";

            user = new ApplicationUser();
            user.Nombre = "Silvia";
            user.Apellido1 = "-";
            user.Apellido2 = "-";
            user.UserName = name;
            user.Email = name;
         var   userResult = UserManager.Create(user, password);

            //Add User Admin to Role Admin
            if (userResult.Succeeded)
            {
                var result = UserManager.AddToRole(user.Id, role);
            }


            //Create Role Alumno if it does not exist
             role = "Alumno";
            if (!RoleManager.RoleExists(role))
            {
                var roleresult = RoleManager.Create(new IdentityRole(role));
            }
        }

        private void MoverCuerpos(AuthContext context)
        {
            var gramaticas = context.Gramaticas.Where(gr=>gr.Cuerpo!=null).ToList();

            foreach(var gramatica in gramaticas)
            {
                GramaticaCuerpo gc = new GramaticaCuerpo()
                {
                    Cuerpo = gramatica.Cuerpo
                };
                context.GramaticaCuerpo.Add(gc);
                context.SaveChanges();

                gramatica.GramaticaCuerpoId = gc.GramaticaCuerpoId;
                gramatica.Cuerpo = null;
                context.Entry(gramatica).State = EntityState.Modified;
                context.SaveChanges();
            }
        }
        private void MoverExtras(AuthContext context)
        {
            var extras = context.Extras.Where(gr => gr.Explicacion != null).ToList();

            foreach (var extra in extras)
            {
                ExtraExplicacion gc = new ExtraExplicacion()
                {
                    Explicacion = extra.Explicacion
                };
                context.ExtraExplicacion.Add(gc);
                context.SaveChanges();

                extra.ExtraExplicacionId = gc.ExtraExplicacionId;
                extra.Explicacion = null;
                context.Entry(extra).State = EntityState.Modified;
                context.SaveChanges();
            }
        }
    }
}
