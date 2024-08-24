using FooBooLooGameAPI.Entities;
using FooBooLooGameAPI.Repositories.Interfaces;
using FooBooLooGameAPI.DTOs;
using FooBooLooGameAPI.Services;
using Moq;
using Xunit;

public class SessionServiceTests
{
    private readonly Mock<ISessionRepository> _mockRepo;
    private readonly SessionService _service;

    public SessionServiceTests()
    {
        _mockRepo = new Mock<ISessionRepository>();
        _service = new SessionService(_mockRepo.Object);
    }

    [Fact]
    public async Task StartSession_ShouldReturnSession()
    {
        var session = new Session { SessionId = 1, GameId = 1, PlayerName = "Player1", Duration = 60 };
        _mockRepo.Setup(repo => repo.CreateSessionAsync(It.IsAny<Session>())).ReturnsAsync(session);

        var result = await _service.StartSessionAsync(1, "Player1", 60);

        Assert.NotNull(result);
        Assert.Equal("Player1", result.PlayerName);
    }

    [Fact]
    public async Task GetNextRandomNumber_ShouldReturnUniqueNumber()
    {
        _mockRepo.Setup(repo => repo.GetUniqueRandomNumberAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(42);

        var result = await _service.GetNextRandomNumberAsync(1, 1, 100);

        Assert.Equal(42, result);
    }

    [Fact]
    public async Task SubmitAnswer_ShouldReturnTrueIfCorrect()
    {
        _mockRepo.Setup(repo => repo.SubmitAnswerAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync(true);

        var result = await _service.SubmitAnswerAsync(1, 15, "FizzBuzz");

        Assert.True(result);
    }

    [Fact]
    public async Task GetSession_ShouldReturnSession()
    {
        var session = new Session { SessionId = 1, GameId = 1, PlayerName = "Player1", Duration = 60 };
        _mockRepo.Setup(repo => repo.GetSessionByIdAsync(It.IsAny<int>())).ReturnsAsync(session);

        var result = await _service.GetSessionAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.SessionId);
    }

    [Fact]
    public async Task EndSessionAsync_ShouldReturnSession()
    {
        var session = new Session { SessionId = 1, GameId = 1, PlayerName = "Player1", Duration = 60 };
        _mockRepo.Setup(repo => repo.EndSessionAsync(It.IsAny<int>())).ReturnsAsync(session);

        var result = await _service.EndSessionAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.SessionId);
    }

    [Fact]
    public async Task GetSessionResultsAsync_ShouldReturnSessionResultDto()
    {
        var sessionResultDto = new SessionResultDto { SessionId = 1, FinalScore = 100, PlayerName = "Player1" };
        _mockRepo.Setup(repo => repo.GetSessionResultsAsync(It.IsAny<int>())).ReturnsAsync(sessionResultDto);

        var result = await _service.GetSessionResultsAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.SessionId);
        Assert.Equal(100, result.FinalScore);
        Assert.Equal("Player1", result.PlayerName);
    }
}

