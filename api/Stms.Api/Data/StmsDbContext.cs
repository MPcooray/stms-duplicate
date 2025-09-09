using Microsoft.EntityFrameworkCore;
using Stms.Api.Models;

namespace Stms.Api.Data;

public class StmsDbContext : DbContext
{
    public StmsDbContext(DbContextOptions<StmsDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Tournament> Tournaments => Set<Tournament>();
    public DbSet<University> Universities => Set<University>();
    public DbSet<Player> Players => Set<Player>();
    public DbSet<Event> Events => Set<Event>();
    public DbSet<Result> Results => Set<Result>();

    // NEW
    public DbSet<ScoringRule> ScoringRules => Set<ScoringRule>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        // Unique email for users
        b.Entity<User>().HasIndex(u => u.Email).IsUnique();

        // Relationships
        b.Entity<University>()
            .HasOne(u => u.Tournament)
            .WithMany(t => t.Universities)
            .HasForeignKey(u => u.TournamentId)
            .OnDelete(DeleteBehavior.Cascade);

        b.Entity<Player>()
            .HasOne(p => p.University)
            .WithMany(u => u.Players)
            .HasForeignKey(p => p.UniversityId)
            .OnDelete(DeleteBehavior.Cascade);

        b.Entity<Result>()
            .HasOne(r => r.Player)
            .WithMany()
            .HasForeignKey(r => r.PlayerId);

        b.Entity<Result>()
            .HasOne(r => r.Event)
            .WithMany()
            .HasForeignKey(r => r.EventId);

        // Composite index used by leaderboard queries
        b.Entity<Result>()
            .HasIndex(r => new { r.EventId, r.TimingMs })
            .HasDatabaseName("IX_Results_EventId_TimingMs");

        // Seed a few Events (same as before)
        b.Entity<Event>().HasData(
            new Event { Id = 1, Code = "50FR", Name = "50m Freestyle",  Distance = 50,  Stroke = "Freestyle" },
            new Event { Id = 2, Code = "100FR", Name = "100m Freestyle", Distance = 100, Stroke = "Freestyle" },
            new Event { Id = 3, Code = "100BK", Name = "100m Backstroke",Distance = 100, Stroke = "Backstroke" }
        );

        // Scoring rules (place -> points)
        b.Entity<ScoringRule>().HasIndex(x => x.Place).IsUnique();
        b.Entity<ScoringRule>().HasData(
            new ScoringRule { Id = 1, Place = 1, Points = 9 },
            new ScoringRule { Id = 2, Place = 2, Points = 7 },
            new ScoringRule { Id = 3, Place = 3, Points = 6 },
            new ScoringRule { Id = 4, Place = 4, Points = 5 },
            new ScoringRule { Id = 5, Place = 5, Points = 4 },
            new ScoringRule { Id = 6, Place = 6, Points = 3 },
            new ScoringRule { Id = 7, Place = 7, Points = 2 },
            new ScoringRule { Id = 8, Place = 8, Points = 1 }
        );
    }
}
