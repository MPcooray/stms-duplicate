using Stms.Api.Models;

namespace Stms.Api.Services;

public interface IUniversityService
{
    Task<List<University>> ListAsync(int tournamentId, CancellationToken ct);
    Task<University> CreateAsync(int tournamentId, University u, CancellationToken ct);
    Task<bool> UpdateAsync(int tournamentId, int id, University incoming, CancellationToken ct);
    Task<bool> DeleteAsync(int tournamentId, int id, CancellationToken ct);
}
