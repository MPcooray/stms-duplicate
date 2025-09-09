namespace Stms.Api.Dtos;

// Per-event leaderboard row
public record EventLeaderboardRow(
    int Rank,
    int Points,
    int PlayerId,
    string PlayerName,
    string University,
    int TimingMs
);

// Tournament totals (sum of points per university)
public record UniversityTotalRow(
    int UniversityId,
    string University,
    int Points
);
