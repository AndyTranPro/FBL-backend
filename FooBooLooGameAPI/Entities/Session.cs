using System;

namespace FooBooLooGameAPI.Entities;

public class Session
{
    public int SessionId { get; set; }
    public int GameId { get; set; }
    public Game Game { get; set; } = null!;
    public string PlayerName { get; set; } = string.Empty;
    public DateTime StartTime { get; set; } = DateTime.UtcNow;
    public int Duration { get; set; }
    public int Score { get; set; }
    public List<SessionNumber> SessionNumbers { get; set; }
    public bool IsEnded { get; set; } = false;
}
