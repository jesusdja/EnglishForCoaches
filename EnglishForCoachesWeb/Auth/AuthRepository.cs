using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using EnglishForCoachesWeb.Database;
using EnglishForCoachesWeb.Models;
using Microsoft.AspNet.Identity.Owin;

namespace EnglishForCoachesWeb.Auth
{
    public class AuthRepository : IDisposable
    {
        private AuthContext _ctx;

        private ApplicationUserManager _userManager;

        public AuthRepository()
        {
            _ctx = new AuthContext();
            _userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
        }

        public async Task<IdentityResult> RegisterUser(UserModel userModel)
        {
            ApplicationUser user = new ApplicationUser
            {
                UserName = userModel.UserName
            };

            var result = await _userManager.CreateAsync(user, userModel.Password);

            return result;
        }

        public async Task<ApplicationUser> FindUser(string userName, string password)
        {
            ApplicationUser user = await _userManager.FindAsync(userName, password);

            return user;
        }
        public ApplicationUser FindById(string id)
        {
            ApplicationUser user = _userManager.FindById(id);

            return user;
        }

        public ApplicationUser FindByName(string id)
        {
            ApplicationUser user = _userManager.FindByName(id);

            return user;
        }

        public IdentityResult Delete(ApplicationUser user)
        {
            IdentityResult result = _userManager.Delete(user);

            return result;
        }

        public IdentityResult Create(ApplicationUser user, string password)
        {
            IdentityResult result =  _userManager.Create(user, password);

            return result;
        }

        public IdentityResult Update(ApplicationUser user )
        {
            IdentityResult result = _userManager.Update(user);            

            return result;
        }
        public IdentityResult UpdatePassword(string userId, string password)
        {
            string resetToken = _userManager.GeneratePasswordResetToken(userId);
            IdentityResult passwordChangeResult = _userManager.ResetPassword(userId, resetToken, password);
            return passwordChangeResult;
        }

        public IdentityResult AddToRole(string id, string role)
        {
            IdentityResult result = _userManager.AddToRole(id, role);

            return result;
        }

        public IQueryable<ApplicationUser> ListarUsuarios()
        {
            var users =  _userManager.GetUsersWithIncludes();

            return users;

          //  return new List<ApplicationUser>();
        }
        

        public void Dispose()
        {
            _ctx.Dispose();
            _userManager.Dispose();

        }
    }
}