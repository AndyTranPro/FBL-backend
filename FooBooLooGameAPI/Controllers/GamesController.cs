using FooBooLooGameAPI.DTOs;
using FooBooLooGameAPI.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FooBooLooGameAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[EnableCors("AllowSpecificOrigin")]
public class GamesController : ControllerBase
{
    private readonly GameService _gameService;

    public GamesController(GameService gameService)
    {
        _gameService = gameService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateGame([FromBody] CreateGameDto dto)
    {
        var game = await _gameService.CreateGameAsync(dto.Name, dto.Author, dto.Min, dto.Max, dto.Rules);
        // if the game already exists
        if (game == null)
        {
            return StatusCode(StatusCodes.Status409Conflict, new { Message = "Game already exists" });
        }
        return CreatedAtAction(nameof(GetGame), new { id = game.GameId }, game);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetGame(int id)
    {
        var game = await _gameService.GetGameAsync(id);
        if (game == null)
        {
            return NotFound();
        }
        return Ok(game);
    }

    [HttpGet]
    public async Task<IActionResult> GetGames()
    {
        var games = await _gameService.GetGamesAsync();
        return Ok(games);
    }
}
