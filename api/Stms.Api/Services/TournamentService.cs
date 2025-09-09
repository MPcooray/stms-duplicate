using Microsoft.EntityFrameworkCore;
using Stms.Api.Data;
using Stms.Api.Models;

namespace Stms.Api.Services;

public class TournamentService : ITournamentService
{
    private readonly StmsDbContext _db;
    public TournamentService(StmsDbContext db) => _db = db;

    public Task<List<Tournament>> GetAllAsync(CancellationToken ct) =>
        _db.Tournaments.OrderBy(t => t.StartDate).ToListAsync(ct);

    public Task<Tournament?> GetByIdAsync(int id, CancellationToken ct) =>
        _db.Tournaments.FindAsync(new object?[] { id }, ct).AsTask();

    public async Task<Tournament> CreateAsync(Tournament t, CancellationToken ct)
    {
        if (t.EndDate < t.StartDate) throw new ArgumentException("EndDate cannot be before StartDate");
        _db.Tournaments.Add(t);
        await _db.SaveChangesAsync(ct);
        return t;
    }

    public async Task<bool> UpdateAsync(int id, Tournament incoming, CancellationToken ct)
    {
        var t = await _db.Tournaments.FindAsync(new object?[] { id }, ct);
        if (t is null) return false;
        if (incoming.EndDate < incoming.StartDate) throw new ArgumentException("EndDate cannot be before StartDate");

        t.Name = incoming.Name;
        t.Venue = incoming.Venue;
        t.StartDate = incoming.StartDate;
        t.EndDate = incoming.EndDate;
        t.Status = incoming.Status;

        await _db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var t = await _db.Tournaments.FindAsync(new object?[] { id }, ct);
        if (t is null) return false;
        _db.Tournaments.Remove(t);
        await _db.SaveChangesAsync(ct);
        return true;
    }
}
