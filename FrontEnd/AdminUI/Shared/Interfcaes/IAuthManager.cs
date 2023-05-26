using AdminUI.Shared.Requests;

namespace AdminUI.Shared.Interfcaes
{
    public interface IAuthManager
    {
        Task<string> LoginUserAsync(LoginUserRequest request);
        Task<string> RegisterUserAsync(RegisterUserRequest request);
    }
}
