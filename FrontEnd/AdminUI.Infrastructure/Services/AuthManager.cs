using System.Net.Http.Json;
using AdminUI.Shared.Interfcaes;
using AdminUI.Shared.Requests;

namespace AdminUI.Infrastructure.Services
{
    public class AuthManager : IAuthManager
    {
        private readonly HttpClient httpClient;
        public AuthManager(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<string> RegisterUserAsync(RegisterUserRequest request)
        {
            var response = await httpClient.PostAsJsonAsync("api/auth/register", request);
            return response is { IsSuccessStatusCode: true } ? "User registered successfully" 
                                                             : "Error registering user";
        }

        public async Task<string> LoginUserAsync(LoginUserRequest request)
        {
            var response = await httpClient.PostAsJsonAsync("api/auth/login", request);
            return response is { IsSuccessStatusCode: true } ? "User registered successfully" 
                                                             : "Error registering user";
        }
    }
}
