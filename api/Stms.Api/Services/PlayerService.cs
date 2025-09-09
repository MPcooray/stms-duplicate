using Microsoft.EntityFrameworkCore;
using Stms.Api.Data;
using Stms.Api.Dtos;
using Stms.Api.Models;

namespace Stms.Api.Services;

public class PlayerService : IPlayerService
{
    private readonly StmsDbContext _db;
    public PlayerService(StmsDbContext db) => _db = db;

    public async Task<List<Player>> ListAsync(int universityId, CancellationToken ct = default)
    {
        return await _db.Players
            .Where(p => p.UniversityId == universityId)
            .OrderBy(p => p.Id)
            .ToListAsync(ct);
    }

    public async Task<Player> CreateAsync(int universityId, PlayerCreateDto dto, CancellationToken ct = default)
    {
        var uniExists = await _db.Universities.AnyAsync(u => u.Id == universityId, ct);
        if (!uniExists) throw new KeyNotFoundException("University not found");

        var p = new Player
        {
            FirstName = dto.FirstName.Trim(),
            LastName  = dto.LastName.Trim(),
            UniversityId = universityId
        };

        _db.Players.Add(p);
        await _db.SaveChangesAsync(ct);
        return p;
    }
}
