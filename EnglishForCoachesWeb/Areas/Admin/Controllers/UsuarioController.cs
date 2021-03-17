using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EnglishForCoachesWeb.Database;
using EnglishForCoachesWeb.Auth;
using EnglishForCoachesWeb.Areas.Admin.Models;
using EnglishForCoachesWeb.Controllers;
using EnglishForCoachesWeb.Database.Ejercicios;
using System.Security.Claims;

namespace EnglishForCoachesWeb.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,AdministradorGrupo")]
    public class UsuarioController : MVCBaseController
    {
        private AuthContext db = new AuthContext();

        // GET: Admin/Usuario
        public ActionResult Grupos()
        {
            UsuarioGruposViewModel viewModel = new UsuarioGruposViewModel();
            
            viewModel.Grupos = db.GrupoUsuarios.OrderBy(x => x.Descripcion).ToList();
            return View(viewModel);
        }

        // GET: Admin/Usuario
        public ActionResult Index()
        {
            AuthRepository list = new AuthRepository();

            UsuarioIndexViewModel viewModel = new UsuarioIndexViewModel();


            var consulta = list.ListarUsuarios();
            if (User.IsInRole("AdministradorGrupo"))
            {
                var GrupoUsuarioId = Convert.ToInt32(((ClaimsIdentity)User.Identity).FindFirst("GrupoUsuario").Value);

                var tipoUsuarioId = (int)TiposUsuariosId.Alumno;
                consulta = consulta.Where(x => x.TipoUsuarioId== tipoUsuarioId && x.GrupoUsuarioId == GrupoUsuarioId);
            }

            if (User.IsInRole("Admin"))
            {

                var tipoUsuarioId1 = (int)TiposUsuariosId.Alumno;
                var tipoUsuarioId2 = (int)TiposUsuariosId.AdministradorGrupo;
                consulta =consulta.Where(x => x.TipoUsuarioId==tipoUsuarioId1|| x.TipoUsuarioId == tipoUsuarioId2);
            }


            viewModel.Usuarios =consulta.OrderBy(x=>x.Apellido1).ToList();
            return View(viewModel);
        }

        // POST: Admin/Usuario/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(UsuarioIndexViewModel viewModel)
        {
            AuthRepository list = new AuthRepository();

            var busqueda = list.ListarUsuarios();
            
            if (User.IsInRole("AdministradorGrupo"))
            {
                var GrupoUsuarioId = Convert.ToInt32(((ClaimsIdentity)User.Identity).FindFirst("GrupoUsuario").Value);

                var tipoUsuarioId = (int)TiposUsuariosId.Alumno;
                busqueda = busqueda.Where(x => x.TipoUsuarioId == tipoUsuarioId && x.GrupoUsuarioId == GrupoUsuarioId);
            }

            if (User.IsInRole("Admin"))
            {

                var tipoUsuarioId1 = (int)TiposUsuariosId.Alumno;
                var tipoUsuarioId2 = (int)TiposUsuariosId.AdministradorGrupo;
                busqueda = busqueda.Where(x => x.TipoUsuarioId == tipoUsuarioId1 || x.TipoUsuarioId == tipoUsuarioId2);
            }

            if (!string.IsNullOrWhiteSpace(viewModel.TextoBusqueda))
            {
                busqueda = busqueda.Where(x => x.Apellido1.Contains(viewModel.TextoBusqueda)||
                x.Apellido2.Contains(viewModel.TextoBusqueda) ||
                x.Nombre.Contains(viewModel.TextoBusqueda) ||
                x.Email.Contains(viewModel.TextoBusqueda));
            }

            viewModel.Usuarios = busqueda.OrderBy(x => x.Apellido1).ToList();
            return View(viewModel);
        }

        // GET: Admin/Usuario/Create
        public ActionResult Create()
        {
            UsuarioCreateViewModel viewModel = new UsuarioCreateViewModel();
            viewModel.InicializarDesplegables();

            

            return View(viewModel);
        }

        // POST: Admin/Usuario/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UsuarioCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                AuthRepository gestorUsuarios = new AuthRepository();
                viewModel.Usuario.UserName = viewModel.Usuario.Email;
                viewModel.Usuario.PuntosTotal = 0;
                viewModel.Usuario.PuntosActual = 0;
                var correcto = true;
                if (User.IsInRole("AdministradorGrupo"))
                {
                    var GrupoUsuarioId = Convert.ToInt32(((ClaimsIdentity)User.Identity).FindFirst("GrupoUsuario").Value);
                    viewModel.Usuario.GrupoUsuarioId = GrupoUsuarioId;
                    viewModel.Usuario.TipoUsuarioId = (int)TiposUsuariosId.Alumno;

                    var grupo = db.GrupoUsuarios.Find(GrupoUsuarioId);
                    if (grupo.NumeroMaximoUsuarios > 0)
                    {
                        var nUsuario = db.Users.Where(us => us.GrupoUsuarioId.HasValue).Count(us => us.GrupoUsuarioId.Value == viewModel.Usuario.GrupoUsuarioId);

                        if (nUsuario >= grupo.NumeroMaximoUsuarios)
                        {
                            correcto = false;
                            ModelState.AddModelError("Usuario.Email", "Ha sobrepasado el número máximo de usuarios del grupo");
                        }
                    }

                }
                if (correcto)
                {
                    var userResult = gestorUsuarios.Create(viewModel.Usuario, viewModel.Password);

                    //Add User Admin to Role Admin
                    if (userResult.Succeeded)
                    {
                        string oldPathAndName = "~/media/upload/avatar/sin-avatar.jpg";
                        string newPathAndName = "~/media/upload/avatar/" + viewModel.Usuario.Id + ".jpg";

                        System.IO.File.Copy(Server.MapPath(oldPathAndName), Server.MapPath(newPathAndName));

                        List<int> listaSubtemasAccesso = new List<int>();
                        if (viewModel.Usuario.BloquearSubtemas)
                        {
                            foreach (var acceso in viewModel.AccesoTemas)
                            {
                                foreach (var accesoSubtema in acceso.SubTemas)
                                {
                                    if (accesoSubtema.Selected)
                                    {
                                        SubTemaAccesoUsuario subtemaAcceso = new SubTemaAccesoUsuario();
                                        subtemaAcceso.AlumnoId = viewModel.Usuario.Id;
                                        subtemaAcceso.FechaAcceso = DateTime.Now;
                                        subtemaAcceso.SubTemaId = Convert.ToInt32(accesoSubtema.Value);

                                        db.SubTemaAccesoUsuarios.Add(subtemaAcceso);
                                        db.SaveChanges();

                                        listaSubtemasAccesso.Add(subtemaAcceso.SubTemaId);
                                    }
                                }
                            }
                            var subtemasAcceso = db.SubTemaAccesoUsuarios.Where(sa => sa.AlumnoId == viewModel.Usuario.Id).Select(sa => sa.SubTema).ToList();
                            var temas = subtemasAcceso.Select(su => su.TemaId);
                            foreach (var temaId in temas)
                            {
                                var subtemaInicial = subtemasAcceso.Where(sa => sa.TemaId == temaId).OrderBy(su => su.Orden).FirstOrDefault();
                                if (subtemaInicial != null)
                                {
                                    if (!db.SubTemaDesbloqueados.Any(sd => sd.SubTemaId == subtemaInicial.SubTemaId && sd.AlumnoId == viewModel.Usuario.Id))
                                    {
                                        SubTemaDesbloqueado desbloqueado = new SubTemaDesbloqueado();
                                        desbloqueado.AlumnoId = viewModel.Usuario.Id;
                                        desbloqueado.FechaDesbloqueo = DateTime.Now;
                                        desbloqueado.SubTemaId = subtemaInicial.SubTemaId;

                                        db.SubTemaDesbloqueados.Add(desbloqueado);
                                        db.SaveChanges();
                                    }
                                }
                            }
                        }

                        List<int> temasCliente = db.ClienteTemas.Where(te => te.ClienteId == viewModel.Usuario.ClienteId).Select(te=>te.TemaId).ToList();

                        List<SubTema> subtemasIniciales = db.SubTemas.Where(sub => temasCliente.Contains(sub.TemaId) && sub.Orden == 1).ToList();
                        foreach (SubTema subtema in subtemasIniciales)
                        {
                            bool anyadir = true;
                            if (viewModel.Usuario.BloquearSubtemas)
                            {
                                if (!listaSubtemasAccesso.Contains(subtema.SubTemaId))
                                {
                                    anyadir = false;
                                }
                            }
                            if (anyadir)
                            {
                                if (!db.SubTemaDesbloqueados.Any(sd => sd.SubTemaId == subtema.SubTemaId && sd.AlumnoId == viewModel.Usuario.Id))
                                {
                                    SubTemaDesbloqueado desbloqueado = new SubTemaDesbloqueado();
                                    desbloqueado.AlumnoId = viewModel.Usuario.Id;
                                    desbloqueado.FechaDesbloqueo = DateTime.Now;
                                    desbloqueado.SubTemaId = subtema.SubTemaId;

                                    db.SubTemaDesbloqueados.Add(desbloqueado);
                                    db.SaveChanges();
                                }
                            }
                        }

                        var result = gestorUsuarios.AddToRole(viewModel.Usuario.Id, "Alumno");
                        if (viewModel.Usuario.TipoUsuarioId == (int)TiposUsuariosId.AdministradorGrupo)
                        {
                            result = gestorUsuarios.AddToRole(viewModel.Usuario.Id, "AdministradorGrupo");
                        }
                        if (result.Succeeded)
                        {
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ModelState.AddModelError("Usuario.Email", userResult.Errors.First());
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Usuario.Email", userResult.Errors.First());
                    }
                }
            }

            viewModel.InicializarDesplegables();
            return View(viewModel);
        }

        // GET: Admin/Usuario/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AuthRepository gestorUsuarios = new AuthRepository();
            ApplicationUser applicationUser = gestorUsuarios.FindById(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }

            UsuarioEditViewModel viewModel = new UsuarioEditViewModel();
            viewModel.Usuario = applicationUser;
            viewModel.InicializarDesplegables();
            return View(viewModel);
        }

        // POST: Admin/Usuario/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UsuarioEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                AuthRepository gestorUsuarios = new AuthRepository();

                ApplicationUser applicationUser = gestorUsuarios.FindById(viewModel.Usuario.Id);
                applicationUser.Nombre = viewModel.Usuario.Nombre;
                applicationUser.Apellido1 = viewModel.Usuario.Apellido1;
                applicationUser.Apellido2 = viewModel.Usuario.Apellido2;
                applicationUser.Email = viewModel.Usuario.Email;
                applicationUser.UserName = viewModel.Usuario.Email;
                applicationUser.GrupoUsuarioId = viewModel.Usuario.GrupoUsuarioId;
                applicationUser.PuntosActual = viewModel.Usuario.PuntosActual;
                applicationUser.PuntosTotal = viewModel.Usuario.PuntosTotal;
                    applicationUser.BloquearSubtemas = viewModel.Usuario.BloquearSubtemas;
                applicationUser.ClienteId = viewModel.Usuario.ClienteId;

                var Accesos = db.SubTemaAccesoUsuarios.Where(sau => sau.AlumnoId == applicationUser.Id).ToList();
                db.SubTemaAccesoUsuarios.RemoveRange(Accesos);
                db.SaveChanges();
                List<int> listaSubtemasAccesso = new List<int>();
                if (viewModel.Usuario.BloquearSubtemas)
                {
                    foreach (var acceso in viewModel.AccesoTemas)
                    {
                        foreach (var accesoSubtema in acceso.SubTemas)
                        {
                            if (accesoSubtema.Selected)
                            {
                                SubTemaAccesoUsuario subtemaAcceso = new SubTemaAccesoUsuario();
                                subtemaAcceso.AlumnoId = viewModel.Usuario.Id;
                                subtemaAcceso.FechaAcceso = DateTime.Now;
                                subtemaAcceso.SubTemaId = Convert.ToInt32(accesoSubtema.Value);

                                db.SubTemaAccesoUsuarios.Add(subtemaAcceso);
                                db.SaveChanges();
                                listaSubtemasAccesso.Add(subtemaAcceso.SubTemaId);
                            }
                        }


                    }
                    var subtemasAcceso = db.SubTemaAccesoUsuarios.Where(sa => sa.AlumnoId == viewModel.Usuario.Id).Select(sa => sa.SubTema).ToList();
                    var temas = subtemasAcceso.Select(su => su.TemaId);
                    foreach (var temaId in temas)
                    {
                        var subtemaInicial = subtemasAcceso.Where(sa => sa.TemaId == temaId).OrderBy(su => su.Orden).FirstOrDefault();
                        if (subtemaInicial != null)
                        {
                            if (!db.SubTemaDesbloqueados.Any(sd => sd.SubTemaId == subtemaInicial.SubTemaId && sd.AlumnoId == viewModel.Usuario.Id))
                            {
                                SubTemaDesbloqueado desbloqueado = new SubTemaDesbloqueado();
                                desbloqueado.AlumnoId = viewModel.Usuario.Id;
                                desbloqueado.FechaDesbloqueo = DateTime.Now;
                                desbloqueado.SubTemaId = subtemaInicial.SubTemaId;

                                db.SubTemaDesbloqueados.Add(desbloqueado);
                                db.SaveChanges();
                            }
                        }
                    }
                }

                List<int> temasCliente = db.ClienteTemas.Where(te => te.ClienteId == viewModel.Usuario.ClienteId).Select(te => te.TemaId).ToList();

                List<SubTema> subtemasIniciales = db.SubTemas.Where(sub => temasCliente.Contains(sub.TemaId) && sub.Orden == 1).ToList();
                foreach (SubTema subtema in subtemasIniciales)
                {
                    bool anyadir = true;
                    if (viewModel.Usuario.BloquearSubtemas)
                    {
                        if (!listaSubtemasAccesso.Contains(subtema.SubTemaId))
                        {
                            anyadir = false;
                        }
                    }
                    if (anyadir)
                    {
                        if (!db.SubTemaDesbloqueados.Any(sd => sd.SubTemaId == subtema.SubTemaId && sd.AlumnoId == viewModel.Usuario.Id))
                        {
                            SubTemaDesbloqueado desbloqueado = new SubTemaDesbloqueado();
                            desbloqueado.AlumnoId = viewModel.Usuario.Id;
                            desbloqueado.FechaDesbloqueo = DateTime.Now;
                            desbloqueado.SubTemaId = subtema.SubTemaId;

                            db.SubTemaDesbloqueados.Add(desbloqueado);
                            db.SaveChanges();
                        }
                    }
                }

                var userResult = gestorUsuarios.Update(applicationUser);

                if (userResult.Succeeded)
                {
                    if (!string.IsNullOrWhiteSpace(viewModel.Password))
                    {
                        gestorUsuarios.UpdatePassword(viewModel.Usuario.Id, viewModel.Password);
                    }
                    return RedirectToAction("Index");
                }
            }
            viewModel.InicializarDesplegables();
            return View(viewModel);
        }

        // GET: Admin/Usuario/CreateGrupo
        public ActionResult CreateGrupo( )
        {
            UsuarioCreateGrupoViewModel viewModel = new UsuarioCreateGrupoViewModel();
            return View(viewModel);
        }

        // POST: Admin/Usuario/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateGrupo(UsuarioCreateGrupoViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                db.GrupoUsuarios.Add(viewModel.GrupoUsuario);
                db.SaveChanges();
                return RedirectToAction("Grupos");
            }
            return View(viewModel);
        }


        // GET: Admin/Usuario/Edit/5
        public ActionResult EditGrupo(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var grupoUsuario = db.GrupoUsuarios.Find(id);
            if (grupoUsuario == null)
            {
                return HttpNotFound();
            }

            UsuarioEditGrupoViewModel viewModel = new UsuarioEditGrupoViewModel();
            viewModel.GrupoUsuario = grupoUsuario;
            return View(viewModel);
        }

        // POST: Admin/Usuario/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditGrupo(UsuarioEditGrupoViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(viewModel.GrupoUsuario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Grupos");
            }
            return View(viewModel);
        }

        // GET: Admin/Usuario/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AuthRepository gestorUsuarios = new AuthRepository();
            ApplicationUser applicationUser = gestorUsuarios.FindById(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            string fullPath = Request.MapPath("~/media/upload/avatar/" + applicationUser.Id + ".jpg");
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
            var Accesos = db.SubTemaAccesoUsuarios.Where(sau => sau.AlumnoId == applicationUser.Id).ToList();
            db.SubTemaAccesoUsuarios.RemoveRange(Accesos);
            db.SaveChanges();

            gestorUsuarios.Delete(applicationUser);
            return RedirectToAction("Index");
        }

        // GET: Admin/Usuario/Delete/5
        public ActionResult DeleteGrupo(int id)
        {
           
      
            if (!db.GrupoUsuarios.Any(gr=>gr.GrupoUsuarioId==id))
            {
                return HttpNotFound();
            }
            if(db.Users.Any(user=>user.GrupoUsuarioId==id))
            {
                return RedirectToAction("Grupos");
            }

            GrupoUsuario grupoUsuario = db.GrupoUsuarios.Find(id);
          
            db.GrupoUsuarios.Remove(grupoUsuario);
            db.SaveChanges();

            return RedirectToAction("Grupos");
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
