using Microsoft.AspNetCore.Identity;

namespace Shared.Extensions
{
    public static class IdentityConfigurationExtensions
    {
        public static void ConfigureIdentityOptions(this IdentityOptions options)
        {
            options.Password = PasswordOptions;
            options.User.RequireUniqueEmail = true;
            options.SignIn = SignInOptions;
            options.Tokens = TokenOptions;
            options.Lockout = LockOutOptions;
        }

        private static readonly TokenOptions TokenOptions = new()
        {
            EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider
        };

        private static readonly PasswordOptions PasswordOptions = new()
        {
            RequireDigit = false,
            RequiredLength = 4,
            RequireNonAlphanumeric = false,
            RequireLowercase = false,
            RequireUppercase = false,
            RequiredUniqueChars = 2,
        };

        private static readonly SignInOptions SignInOptions = new()
        {
            RequireConfirmedPhoneNumber = false,
            RequireConfirmedAccount = false,
            RequireConfirmedEmail = false,
        };

        private static readonly LockoutOptions LockOutOptions = new()
        {
            AllowedForNewUsers = false,
            MaxFailedAccessAttempts = 7,
            DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2),
        };
    }
}
