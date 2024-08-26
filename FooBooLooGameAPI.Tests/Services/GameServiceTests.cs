using FooBooLooGameAPI.Entities;
using FooBooLooGameAPI.Repositories.Interfaces;
using FooBooLooGameAPI.Services;
using Moq;
using Xunit;

public class GameServiceTests
{
    private readonly Mock<IGameRepository> _mockRepo;
    private readonly GameService _service;

    public GameServiceTests()
    {
        _mockRepo = new Mock<IGameRepository>();
        _service = new GameService(_mockRepo.Object);
    }

    [Fact]
    public async Task CreateGame_ShouldReturnGame()
    {
        var game = new Game { Name = "FooBooLoo", Author = "X", Min = 1, Max = 100 };
        _mockRepo.Setup(repo => repo.CreateGameAsync(It.IsAny<Game>())).ReturnsAsync(game);

        var result = await _service.CreateGameAsync(game.Name, game.Author, game.Min, game.Max, new Dictionary<int, string>());

        Assert.NotNull(result);
        Assert.Equal(game.Name, result.Name);
        _mockRepo.Verify(repo => repo.CreateGameAsync(It.IsAny<Game>()), Times.Once);
    }

    [Fact]
    public async Task GetGames_ShouldReturnListOfGames()
    {
        var games = new List<Game>
        {
            new Game { Name = "TestGame1", Author = "Author1", Min = 1, Max = 100 },
            new Game { Name = "TestGame2", Author = "Author2", Min = 1, Max = 100 }
        };

        _mockRepo.Setup(repo => repo.GetGamesAsync()).ReturnsAsync(games);

        var result = await _service.GetGamesAsync();

        Assert.Equal(2, result.Count());
        Assert.Equal("TestGame1", result.First().Name);
    }
}

