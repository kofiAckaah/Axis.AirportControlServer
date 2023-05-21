namespace Shared.Interfaces
{
    public interface ICurrentUserService
    {
        Guid UserId { get; }
        List<KeyValuePair<string, string>>? Claims { get; set; }
    }
}
