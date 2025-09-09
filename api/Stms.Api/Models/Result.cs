namespace Stms.Api.Models;

public class Result
{
    public int Id { get; set; }

    public int PlayerId { get; set; }
    public Player Player { get; set; } = default!;

    public int EventId { get; set; }
    public Event Event { get; set; } = default!;

    public int TimingMs { get; set; }
    public int? Heat { get; set; }
    public int? Lane { get; set; }

    // NEW
    public int? Rank { get; set; }   // 1,2,3,… (null until computed)
    public int Points { get; set; }  // computed from rank
}
