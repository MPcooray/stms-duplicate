using Microsoft.EntityFrameworkCore;
using Stms.Api.Data;
using Stms.Api.Models;

public interface IResultService
{
    Task<Result> CreateAsync(int playerId, int eventId, int timingMs, int? heat, int? lane, CancellationToken ct);
    Task<Result> UpdateAsync(int id, int timingMs, int? heat, int? lane, CancellationToken ct);
}

public class ResultService : IResultService
{
    private readonly StmsDbContext _db;
    public ResultService(StmsDbContext db) => _db = db;

    public async Task<Result> CreateAsync(int playerId, int eventId, int timingMs, int? heat, int? lane, CancellationToken ct)
    {
        var res = new Result
        {
            PlayerId = playerId,
            EventId  = eventId,
            TimingMs = timingMs,
            Heat     = heat,
            Lane     = lane
        };
        _db.Results.Add(res);
        await _db.SaveChangesAsync(ct);
        return res;
    }

    public async Task<Result> UpdateAsync(int id, int timingMs, int? heat, int? lane, CancellationToken ct)
    {
        var res = await _db.Results.FirstOrDefaultAsync(r => r.Id == id, ct)
                  ?? throw new KeyNotFoundException($"Result {id} not found");

        res.TimingMs = timingMs;
        res.Heat     = heat;
        res.Lane     = lane;

        await _db.SaveChangesAsync(ct);
        return res; // <-- return entity so controller can read EventId
    }
}
