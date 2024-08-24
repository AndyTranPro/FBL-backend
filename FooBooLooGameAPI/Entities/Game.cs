using System;

namespace FooBooLooGameAPI.Entities;

public class Game
{
    public int GameId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int Min { get; set; }
    public int Max { get; set; }
    public Dictionary<int, string> RuleSet { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
