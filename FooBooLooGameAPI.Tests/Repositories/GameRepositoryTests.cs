using FooBooLooGameAPI.Data;
using FooBooLooGameAPI.Entities;
using FooBooLooGameAPI.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class GameRepositoryTests
{
    private readonly GameDbContext _context;
    private readonly GameRepository _repository;
    
    public GameRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<GameDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Use a unique database name for each test run
            .Options;

        _context = new GameDbContext(options);

        // Seed data for tests
        _context.Games.Add(new Game
        {
            Name = "TestGame",
            Author = "TestAuthor",
            RuleSet = new Dictionary<int, string> { { 3, "Fizz" }, { 5, "Buzz" } }
        });
        _context.SaveChanges();
        _repository = new GameRepository(_context);
    }

    [Fact]
    public async Task CreateGame_ShouldAddGame()
    {
        var newGame = new Game
        {
            Name = "NewGame",
            Author = "NewAuthor",
            RuleSet = new Dictionary<int, string> { { 2, "Foo" }, { 4, "Bar" } }
        };

        var result = await _repository.CreateGameAsync(newGame);

        Assert.NotNull(result);
        Assert.Equal("NewGame", result.Name);
        Assert.Equal(2, await _context.Games.CountAsync());
    }

    [Fact]
    public async Task CreateGame_ShouldReturnNullSinceGameAlreadyExists()
    {
        var newGame = new Game
        {
            Name = "TestGame",
            Author = "TestAuthor",
            RuleSet = new Dictionary<int, string> { { 3, "Fizz" }, { 5, "Buzz" } }
        };
        var result = await _repository.CreateGameAsync(newGame);
        Assert.Null(result);
    }

    [Fact]
    public async Task GetGames_ShouldReturnAllGames()
    {
        var games = await _repository.GetGamesAsync();
        Assert.NotNull(games);
        Assert.Single(games);
    }

    [Fact]
    public async Task GetGameById_ShouldReturnGame()
    {
        var game = await _repository.GetGameByIdAsync(1);
        Assert.NotNull(game);
        Assert.Equal("TestGame", game.Name);
    }
}

