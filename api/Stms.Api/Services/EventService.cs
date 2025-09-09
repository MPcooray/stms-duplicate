using Microsoft.EntityFrameworkCore;
using Stms.Api.Data;
using Stms.Api.Models;

namespace Stms.Api.Services;

public class EventService : IEventService
{
    private readonly StmsDbContext _db;
    public EventService(StmsDbContext db) => _db = db;

    public Task<List<Event>> ListAsync(CancellationToken ct) =>
        _db.Events.OrderBy(e => e.Id).ToListAsync(ct);
}
