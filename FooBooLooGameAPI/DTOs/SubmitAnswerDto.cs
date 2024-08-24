using System;

namespace FooBooLooGameAPI.DTOs;

public class SubmitAnswerDto
{
  public int Number { get; set; }
  public string Answer { get; set; } = string.Empty;
}
