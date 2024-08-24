using System;

namespace FooBooLooGameAPI.DTOs;

public class CreateGameDto
{
    public string Name { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int Min { get; set; }
    public int Max { get; set; }
    public Dictionary<int, string> Rules { get; set; } = new();
}
