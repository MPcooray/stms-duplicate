using Stms.Api.Models;

namespace Stms.Api.Services;

public interface IEventService
{
    Task<List<Event>> ListAsync(CancellationToken ct);
}
