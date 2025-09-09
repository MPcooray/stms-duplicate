using Microsoft.EntityFrameworkCore;
using Stms.Api.Data;
using Stms.Api.Models;

namespace Stms.Api.Services;

public class RankingService : IRankingService
{
    private readonly StmsDbContext _db;
    public RankingService(StmsDbContext db) => _db = db;

    public async Task RecalculateForEvent(int eventId, CancellationToken ct = default)
    {
        // Load results for the event ordered by time (fastest first)
        var results = await _db.Results
            .Where(r => r.EventId == eventId)
            .OrderBy(r => r.TimingMs)
            .ToListAsync(ct);

        // Load scoring rules once (place -> points)
        var ruleMap = await _db.ScoringRules
            .ToDictionaryAsync(x => x.Place, x => x.Points, ct);

        // Dense/competition ranking with ties (same time => same rank)
        int position = 0;
        int rank = 0;
        int? prevTime = null;

        foreach (var r in results)
        {
            position++;
            if (prevTime is null || r.TimingMs > prevTime.Value)
                rank = position;

            r.Rank = rank;
            r.Points = ruleMap.TryGetValue(rank, out var pts) ? pts : 0;

            prevTime = r.TimingMs;
        }

        await _db.SaveChangesAsync(ct);
    }
}
