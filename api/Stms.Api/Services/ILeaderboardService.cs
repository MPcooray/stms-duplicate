using Stms.Api.Dtos;

namespace Stms.Api.Services;

public interface ILeaderboardService
{
    Task<List<EventLeaderboardRow>> GetEventLeaderboard(int eventId, CancellationToken ct = default);
    Task<List<UniversityTotalRow>> GetTournamentUniversityTotals(int tournamentId, CancellationToken ct = default);
}
    