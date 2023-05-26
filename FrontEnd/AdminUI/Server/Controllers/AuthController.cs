using System.Net;
using AdminUI.Shared.Interfcaes;
using AdminUI.Shared.Requests;
using Microsoft.AspNetCore.Mvc;

namespace AdminUI.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthController : Controller
    {
        private readonly IAuthService authService;
        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }
        [Route("register")]
        public async Task<IActionResult> RegisterUserAsync(RegisterUserRequest request)
        {
            var result = await authService.RegisterUserAsync(request);
            return result == HttpStatusCode.OK ? Ok() : (IActionResult)StatusCode((int)result);
        }

        [Route("login")]
        public async Task<IActionResult> LoginUserAsync(LoginUserRequest request)
        {
            var result = await authService.LoginUserAsync(request);
            return result == HttpStatusCode.OK ? Ok() : (IActionResult)StatusCode((int)result);
        }
    }
}
