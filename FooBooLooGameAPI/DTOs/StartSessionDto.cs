using System;

namespace FooBooLooGameAPI.DTOs;

public class StartSessionDto
{
  public int GameId { get; set; }
  public string PlayerName { get; set; } = string.Empty;
  public int Duration { get; set; }
}

