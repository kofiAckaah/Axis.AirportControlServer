using AdminUI.Shared.Requests;

namespace AdminUI.Client.Pages.Auth
{
    public partial class Login
    {
        private LoginUserRequest loginRequest = new();

        private async void SubmitOnValid()
        {
            var result = await authManager.LoginUserAsync(loginRequest);
            snackBar.Add(result);
        }
    }
}
