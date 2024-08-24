using FooBooLooGameAPI.Controllers;
using FooBooLooGameAPI.DTOs;
using FooBooLooGameAPI.Entities;
using FooBooLooGameAPI.Repositories.Interfaces;
using FooBooLooGameAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

public class GamesControllerTests
{
    private readonly Mock<IGameRepository> _mockRepo;
    private readonly GameService _gameService;
    private readonly GamesController _controller;

    public GamesControllerTests()
    {
        _mockRepo = new Mock<IGameRepository>();
        _gameService = new GameService(_mockRepo.Object);
        _controller = new GamesController(_gameService);
    }

    [Fact]
    public async Task CreateGame_ShouldReturnCreatedAtAction()
    {
        var newGame = new Game { GameId = 1, Name = "TestGame", Author = "Author" };
        var dto = new CreateGameDto { Name = "TestGame", Author = "Author", Rules = new Dictionary<int, string>() };

        _mockRepo.Setup(repo => repo.CreateGameAsync(It.IsAny<Game>())).ReturnsAsync(newGame);

        var result = await _controller.CreateGame(dto) as CreatedAtActionResult;

        Assert.NotNull(result);
        Assert.Equal("GetGame", result!.ActionName);
        Assert.Equal(1, (result.Value as Game)!.GameId);
    }

    [Fact]
    public async Task CreateGame_ShouldReturnConflictResult()
    {
        var dto = new CreateGameDto { Name = "TestGame", Author = "Author", Rules = new Dictionary<int, string>() };

        _mockRepo.Setup(repo => repo.CreateGameAsync(It.IsAny<Game>())).ReturnsAsync((Game?)null);

        var result = await _controller.CreateGame(dto) as ObjectResult;

        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status409Conflict, result!.StatusCode);
    }

    [Fact]
    public async Task GetGames_ShouldReturnOkResultWithListOfGames()
    {
        var games = new List<Game> { new Game { GameId = 1, Name = "TestGame", Author = "Author" } };
        _mockRepo.Setup(repo => repo.GetGamesAsync()).ReturnsAsync(games);

        var result = await _controller.GetGames() as OkObjectResult;

        Assert.NotNull(result);
        var returnedGames = Assert.IsType<List<Game>>(result!.Value);
        Assert.Single(returnedGames);
    }

    [Fact]
    public async Task GetGame_ShouldReturnOkResultWithGame()
    {
        var game = new Game { GameId = 1, Name = "TestGame", Author = "Author" };
        _mockRepo.Setup(repo => repo.GetGameByIdAsync(1)).ReturnsAsync(game);

        var result = await _controller.GetGame(1) as OkObjectResult;

        Assert.NotNull(result);
        var returnedGame = Assert.IsType<Game>(result!.Value);
        Assert.Equal("TestGame", returnedGame.Name);
    }

    [Fact]
    public async Task GetGame_ShouldReturnNotFoundResult()
    {
        _mockRepo.Setup(repo => repo.GetGameByIdAsync(1)).ReturnsAsync((Game?)null);

        var result = await _controller.GetGame(1) as NotFoundResult;

        Assert.NotNull(result);
    }
}