namespace Stms.Api.Models;

public class ScoringRule
{
    public int Id { get; set; }
    public int Place { get; set; }   // 1,2,3...
    public int Points { get; set; }  // e.g. 9,7,6...
}
