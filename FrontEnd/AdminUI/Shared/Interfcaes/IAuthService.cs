using AdminUI.Shared.Requests;
using System.Net;

namespace AdminUI.Shared.Interfcaes
{
    public interface IAuthService
    {
        Task<HttpStatusCode> LoginUserAsync(LoginUserRequest request);
        Task<HttpStatusCode> RegisterUserAsync(RegisterUserRequest request);
    }
}
