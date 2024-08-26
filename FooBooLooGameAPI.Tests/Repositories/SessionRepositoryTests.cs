using FooBooLooGameAPI.Data;
using FooBooLooGameAPI.Entities;
using FooBooLooGameAPI.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class SessionRepositoryTests
{
    private readonly GameDbContext _context;
    private readonly SessionRepository _repository;

    public SessionRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<GameDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new GameDbContext(options);

        // Seed data for tests
        var game = new Game
        {
            Name = "TestGame",
            Author = "TestAuthor",
            Min = 1,
            Max = 100,
            RuleSet = new Dictionary<int, string> { { 3, "Fizz" }, { 5, "Buzz" } }
        };

        _context.Games.Add(game);
        _context.Sessions.Add(new Session
        {
            Game = game,
            Score = 0,
            PlayerName = "TestPlayer"
        });
        _context.SaveChanges();
        _repository = new SessionRepository(_context);
    }

    [Fact]
    public async Task CreateSession_ShouldAddSession()
    {
        var newSession = new Session
        {
            GameId = 1,
            Score = 0
        };

        var result = await _repository.CreateSessionAsync(newSession);

        Assert.NotNull(result);
        Assert.Equal(2, await _context.Sessions.CountAsync());
    }

    [Fact]
    public async Task GetSessionById_ShouldReturnSession()
    {
        var session = await _repository.GetSessionByIdAsync(1);
        Assert.NotNull(session);
        Assert.Equal(1, session.SessionId);
    }

    [Fact]
    public async Task GetUniqueRandomNumber_ShouldReturnUniqueNumber()
    {
        var uniqueNumber = await _repository.GetUniqueRandomNumberAsync(1, 1, 10);
        Assert.InRange(uniqueNumber, 1, 10);
    }

    [Fact]
    public async Task SubmitAnswer_ShouldReturnTrueForCorrectAnswer()
    {
        var isCorrect = await _repository.SubmitAnswerAsync(1, 3, "Fizz");
        Assert.True(isCorrect);
    }

    [Fact]
    public async Task SubmitAnswer_ShouldReturnFalseForIncorrectAnswer()
    {
        var isCorrect = await _repository.SubmitAnswerAsync(1, 3, "Buzz");
        Assert.False(isCorrect);
    }

    [Fact]
    public async Task SubmitAnswer_ShouldCreateSessionNumber()
    {
        // Submit correct answer
        var isCorrect = await _repository.SubmitAnswerAsync(1, 3, "Fizz");

        Assert.True(isCorrect);
        var sessionNumber = await _context.SessionNumbers.FirstOrDefaultAsync(sn => sn.SessionId == 1 && sn.NumberServed == 3);
        Assert.NotNull(sessionNumber);
        Assert.Equal(1, sessionNumber.SessionId);
        Assert.Equal(3, sessionNumber.NumberServed);
        Assert.True(sessionNumber.IsCorrect);
        Assert.True(sessionNumber.SessionNumberId > 0);
    }

    [Fact]
    public async Task SubmitAnswer_ShouldReturnFalseForNonExistentSession()
    {
        // Give a non-existent session ID
        var result = await _repository.SubmitAnswerAsync(999, 3, "Fizz");
        Assert.False(result);
    }

    [Fact]
    public async Task CreateSession_ShouldAddSessionWithSessionNumbers()
    {
        // create a session with session numbers
        var newSession = new Session
        {
            GameId = 1,
            Score = 0,
            SessionNumbers = new List<SessionNumber>
            {
                new SessionNumber { NumberServed = 1, IsCorrect = true },
                new SessionNumber { NumberServed = 2, IsCorrect = false }
            }
        };

        var result = await _repository.CreateSessionAsync(newSession);

        Assert.NotNull(result);
        Assert.Equal(2, await _context.Sessions.CountAsync());
        var session = await _context.Sessions.Include(s => s.SessionNumbers).FirstOrDefaultAsync(s => s.SessionId == result.SessionId);
        Assert.NotNull(session);
        Assert.Equal(2, session.SessionNumbers.Count);
        Assert.Equal(1, session.SessionNumbers[0].NumberServed);
        Assert.True(session.SessionNumbers[0].IsCorrect);
        Assert.Equal(2, session.SessionNumbers[1].NumberServed);
        Assert.False(session.SessionNumbers[1].IsCorrect);
    }

    [Fact]
    public async Task EndSession_ShouldMarkSessionAsEnded()
    {
        var session = await _repository.GetSessionByIdAsync(1);
        Assert.NotNull(session);
        Assert.False(session.IsEnded);

        var result = await _repository.EndSessionAsync(1);

        Assert.NotNull(result);
        Assert.True(result.IsEnded);
    }

    [Fact]
    public async Task EndSession_ShouldReturnNullForNonExistentSession()
    {
        var result = await _repository.EndSessionAsync(999);
        Assert.Null(result);
    }

    [Fact]
    public async Task GetSessionResults_ShouldReturnResultsForEndedSession()
    {
        await _repository.EndSessionAsync(1);
        await _repository.SubmitAnswerAsync(1, 3, "Fizz");

        var result = await _repository.GetSessionResultsAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.SessionId);
        Assert.Equal("TestPlayer", result.PlayerName);
        Assert.Equal(1, result.TotalQuestions);
        Assert.Equal(1, result.CorrectAnswers);
        Assert.Equal(0, result.IncorrectAnswers);
        Assert.Equal(1, result.FinalScore);
    }

    [Fact]
    public async Task GetSessionResults_ShouldReturnNullForNonEndedSession()
    {
        var result = await _repository.GetSessionResultsAsync(1);
        Assert.Null(result);
    }

    [Fact]
    public async Task GetSessionResults_ShouldReturnNullForNonExistentSession()
    {
        var result = await _repository.GetSessionResultsAsync(999);
        Assert.Null(result);
    }

}