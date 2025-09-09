namespace Stms.Api.Services;

public interface IRankingService
{
    Task RecalculateForEvent(int eventId, CancellationToken ct = default);
}
