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
using EnglishForCoachesWeb.Controllers;
using EnglishForCoachesWeb.Database.Ejercicios;
using EnglishForCoachesWeb.Areas.Alumno.Models;
using EnglishForCoachesWeb.Helpers;

namespace EnglishForCoachesWeb.Areas.Alumno.Controllers
{
    [Authorize(Roles = "Alumno,Admin")]
    public class UsuarioController : MVCBaseController
    {
        private AuthContext db = new AuthContext();

     
        // GET: Admin/Usuario/Edit/5
        public ActionResult Edit()
        {
            AuthRepository authRepository = new AuthRepository();
            ApplicationUser user = authRepository.FindByName(User.Identity.Name);

            UsuarioEditViewModel viewModel = new UsuarioEditViewModel();
            viewModel.Usuario = user;
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
                bool isValid = true;
                if (!string.IsNullOrWhiteSpace(viewModel.Password) || !string.IsNullOrWhiteSpace(viewModel.PasswordRepeat))
                {
                    if (viewModel.Password != viewModel.PasswordRepeat)
                    {
                        ModelState.AddModelError("Password", "Las contraseñas no coinciden");
                        isValid = false;
                    }                    
                }
                if (isValid)
                {
                    if (viewModel.Avatar != null)
                    {
                        FileUploadHelper.SubirAvatar(viewModel.Avatar, Server.MapPath(Url.Content("~/media/upload/avatar/")) + viewModel.Usuario.Id + ".jpg");
                    //    string nameAndLocation = "~/media/upload/avatar/" + viewModel.Usuario.Id + ".jpg";
                      //  viewModel.Avatar.SaveAs(Server.MapPath(nameAndLocation));
                    }

                    AuthRepository gestorUsuarios = new AuthRepository();

                    ApplicationUser applicationUser = gestorUsuarios.FindById(viewModel.Usuario.Id);
                    applicationUser.Nombre = viewModel.Usuario.Nombre;
                    applicationUser.Apellido1 = viewModel.Usuario.Apellido1;
                    applicationUser.Apellido2 = viewModel.Usuario.Apellido2;
                    applicationUser.Email = viewModel.Usuario.Email;
                    applicationUser.UserName = viewModel.Usuario.Email;

                    var userResult = gestorUsuarios.Update(applicationUser);

                    if (userResult.Succeeded)
                    {
                        if (!string.IsNullOrWhiteSpace(viewModel.Password))
                        {


                            var result =gestorUsuarios.UpdatePassword(viewModel.Usuario.Id, viewModel.Password);
                            result.ToString();
                            if(!result.Succeeded)
                            {
                                ModelState.AddModelError("Password", result.Errors.First().ToString());
                                return View(viewModel);
                            }
                        }
                        return RedirectToAction("Edit");
                    }
                }
            }
            return View(viewModel);
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
