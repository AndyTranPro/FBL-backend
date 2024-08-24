using FooBooLooGameAPI.DTOs;
using FooBooLooGameAPI.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FooBooLooGameAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[EnableCors("AllowSpecificOrigin")]
public class SessionsController : ControllerBase
{
    private readonly SessionService _sessionService;

    public SessionsController(SessionService sessionService)
    {
        _sessionService = sessionService;
    }

    [HttpPost]
    public async Task<IActionResult> StartSession([FromBody] StartSessionDto dto)
    {
        var session = await _sessionService.StartSessionAsync(dto.GameId, dto.PlayerName, dto.Duration);
        return CreatedAtAction(nameof(GetSession), new { id = session.SessionId }, session);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSession(int id)
    {
        var session = await _sessionService.GetSessionAsync(id);
        if (session == null)
        {
            return NotFound();
        }
        return Ok(session);
    }

    [HttpGet("{sessionId}/next-number")]
    public async Task<IActionResult> GetNextNumber(int sessionId)
    {
        // get the min and max values from the game
        var session = await _sessionService.GetSessionAsync(sessionId);
        var min = session.Game.Min;
        var max = session.Game.Max;
        var number = await _sessionService.GetNextRandomNumberAsync(sessionId, min, max);
        return Ok(number);
    }

    [HttpPost("{sessionId}/submit-answer")]
    public async Task<IActionResult> SubmitAnswer(int sessionId, [FromBody] SubmitAnswerDto dto)
    {
        var isCorrect = await _sessionService.SubmitAnswerAsync(sessionId, dto.Number, dto.Answer);
        return Ok(new { isCorrect });
    }

    [HttpPost("{sessionId}/end")]
    public async Task<IActionResult> EndSession(int sessionId)
    {
        var session = await _sessionService.EndSessionAsync(sessionId);
        if (session == null)
        {
            return NotFound(new { Message = "Session not found or already ended" });
        }

        return Ok(new { Message = "Session ended successfully" });
    }

    [HttpGet("{sessionId}/results")]
    public async Task<IActionResult> GetSessionResults(int sessionId)
    {
        var results = await _sessionService.GetSessionResultsAsync(sessionId);
        if (results == null)
        {
            return NotFound(new { Message = "Session not found or not ended yet" });
        }

        return Ok(results);
    }
}

