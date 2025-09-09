using Stms.Api.Models;

namespace Stms.Api.Services;

public interface IResultService
{
    Task<List<Result>> ListAsync(CancellationToken ct);
     Task<Result> CreateAsync(int playerId, int eventId, int timingMs, int? heat, int? lane, CancellationToken ct);
    Task<Result> UpdateAsync(int id, int timingMs, int? heat, int? lane, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
}
