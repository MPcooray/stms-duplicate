using Stms.Api.Models;

namespace Stms.Api.Services;

public interface ITournamentService
{
    Task<List<Tournament>> GetAllAsync(CancellationToken ct);
    Task<Tournament?> GetByIdAsync(int id, CancellationToken ct);
    Task<Tournament> CreateAsync(Tournament t, CancellationToken ct);
    Task<bool> UpdateAsync(int id, Tournament incoming, CancellationToken ct);
    Task<bool> DeleteAsync(int id, CancellationToken ct);
}
