using Microsoft.EntityFrameworkCore;
using Stms.Api.Data;
using Stms.Api.Models;

namespace Stms.Api.Services;

public class UniversityService : IUniversityService
{
    private readonly StmsDbContext _db;
    public UniversityService(StmsDbContext db) => _db = db;

    public Task<List<University>> ListAsync(int tournamentId, CancellationToken ct) =>
        _db.Universities.Where(u => u.TournamentId == tournamentId)
           .OrderBy(u => u.Id).ToListAsync(ct);

    public async Task<University> CreateAsync(int tournamentId, University u, CancellationToken ct)
    {
        u.TournamentId = tournamentId;
        if (string.IsNullOrWhiteSpace(u.Name))
            throw new ArgumentException("Name is required");

        bool exists = await _db.Universities
            .AnyAsync(x => x.TournamentId == tournamentId && x.Name == u.Name, ct);
        if (exists) throw new ArgumentException("University already exists for this tournament.");

        _db.Universities.Add(u);
        await _db.SaveChangesAsync(ct);
        return u;
    }

    public async Task<bool> UpdateAsync(int tournamentId, int id, University incoming, CancellationToken ct)
    {
        var u = await _db.Universities.FirstOrDefaultAsync(
            x => x.Id == id && x.TournamentId == tournamentId, ct);
        if (u is null) return false;
        if (string.IsNullOrWhiteSpace(incoming.Name))
            throw new ArgumentException("Name is required");
        u.Name = incoming.Name;
        await _db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteAsync(int tournamentId, int id, CancellationToken ct)
    {
        var u = await _db.Universities.FirstOrDefaultAsync(
            x => x.Id == id && x.TournamentId == tournamentId, ct);
        if (u is null) return false;
        _db.Universities.Remove(u);
        await _db.SaveChangesAsync(ct);
        return true;
    }
}
