using Microsoft.AspNetCore.Http;
using Shared.Interfaces;
using System.Security.Claims;

namespace Shared.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public CurrentUserService(IHttpContextAccessor contextAccessor)
        {
            var user = contextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (Guid.TryParse(user, out var userId))
                UserId = userId;
            Claims = contextAccessor.HttpContext?.User?.Claims.AsEnumerable()
                .Select(claim => new KeyValuePair<string, string>(claim.Type, claim.Value))
                .ToList();
        }

        public Guid UserId { get; }
        public List<KeyValuePair<string, string>>? Claims { get; set; }
    }
}
