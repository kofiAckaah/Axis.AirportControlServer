using System.Runtime.InteropServices;
using AdminUI.Shared.Requests;

namespace AdminUI.Client.Pages.Auth
{
    public partial class Register
    {
        private RegisterUserRequest registerRequest = new();

        private async void SubmitOnValid()
        {
            var result = await authManager.RegisterUserAsync(registerRequest);
            snackBar.Add(result);
        }
    }
}
