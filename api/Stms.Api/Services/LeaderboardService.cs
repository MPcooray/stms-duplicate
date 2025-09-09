using Microsoft.EntityFrameworkCore;
using Stms.Api.Data;
using Stms.Api.Dtos;

namespace Stms.Api.Services;

public class LeaderboardService : ILeaderboardService
{
    private readonly StmsDbContext _db;
    public LeaderboardService(StmsDbContext db) => _db = db;

    // 1) Per-event leaderboard (ranked, then time)
    public async Task<List<EventLeaderboardRow>> GetEventLeaderboard(
        int eventId, CancellationToken ct = default)
    {
        var rows = await _db.Results
            .AsNoTracking()
            .Where(r => r.EventId == eventId)
            .OrderBy(r => r.Rank).ThenBy(r => r.TimingMs)
            .Select(r => new EventLeaderboardRow(
                r.Rank ?? 0,
                r.Points,
                r.PlayerId,
                r.Player!.FirstName + " " + r.Player!.LastName,
                r.Player!.University!.Name,
                r.TimingMs
            ))
            .ToListAsync(ct);

        return rows;
    }

    // 2) Tournament totals by university (sum of points)
    public async Task<List<UniversityTotalRow>> GetTournamentUniversityTotals(
        int tournamentId, CancellationToken ct = default)
    {
        // Project to a flat shape first (safer translation), then group.
        var rows = await _db.Results
            .AsNoTracking()
            .Select(r => new
            {
                r.Points,
                UniversityId   = r.Player!.UniversityId,
                UniversityName = r.Player!.University!.Name,
                TournamentId   = r.Player!.University!.TournamentId
            })
            .Where(x => x.TournamentId == tournamentId)
            .GroupBy(x => new { x.UniversityId, x.UniversityName })
            .Select(g => new UniversityTotalRow(
                g.Key.UniversityId,
                g.Key.UniversityName,
                g.Sum(x => x.Points)
            ))
            .OrderByDescending(r => r.Points).ThenBy(r => r.University)
            .ToListAsync(ct);

        return rows;
    }
}
