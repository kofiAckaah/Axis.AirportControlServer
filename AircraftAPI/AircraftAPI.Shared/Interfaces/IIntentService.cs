namespace AircraftAPI.Shared.Interfaces
{
    public interface IIntentService
    {
        Task<bool> CanApproachAsync(CancellationToken token);
        Task CancelApproachAsync(CancellationToken token = default);
        Task CancelTakeOff(CancellationToken token);
        Task<bool> CanTakeOff(CancellationToken token);
        Task<bool> TakeOff(CancellationToken token = default);
    }
}
