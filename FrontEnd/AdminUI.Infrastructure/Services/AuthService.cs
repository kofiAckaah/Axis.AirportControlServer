using System.Net;
using System.Runtime.InteropServices;
using AdminUI.Shared.Interfcaes;
using AdminUI.Shared.Requests;
using BackEnd.DataDomain.Entities;
using Microsoft.AspNetCore.Identity;

namespace AdminUI.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public async Task<HttpStatusCode> RegisterUserAsync(RegisterUserRequest request)
        {
            var userExists = await userManager.FindByEmailAsync(request.Username);
            if (userExists != null)
                return HttpStatusCode.Conflict;
            var user = new ApplicationUser
            {
                UserName = request.Username,
                Email = request.Username,
            };

            var result = await userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                var sR = await signInManager.PasswordSignInAsync(user, request.Password, false, false);
                if (sR.Succeeded)
                    return HttpStatusCode.OK;
                else
                    return HttpStatusCode.InternalServerError;
            }

            return HttpStatusCode.InternalServerError;
        }

        public async Task<HttpStatusCode> LoginUserAsync(LoginUserRequest request)
        {
            var user = await userManager.FindByEmailAsync(request.Username);
            if (user == null || !await userManager.CheckPasswordAsync(user, request.Password))
                return HttpStatusCode.InternalServerError;

            var sR = await signInManager.PasswordSignInAsync(user, request.Password, false, false);
            
            return sR.Succeeded ? HttpStatusCode.OK : HttpStatusCode.InternalServerError;
        }
    }
}
