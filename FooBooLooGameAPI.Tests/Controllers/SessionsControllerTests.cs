using FooBooLooGameAPI.Controllers;
using FooBooLooGameAPI.DTOs;
using FooBooLooGameAPI.Entities;
using FooBooLooGameAPI.Repositories.Interfaces;
using FooBooLooGameAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

public class SessionsControllerTests
{
    private readonly Mock<ISessionRepository> _mockRepo;
    private readonly SessionService _sessionService;
    private readonly SessionsController _controller;

    public SessionsControllerTests()
    {
        _mockRepo = new Mock<ISessionRepository>();
        _sessionService = new SessionService(_mockRepo.Object);
        _controller = new SessionsController(_sessionService);
    }

    [Fact]
    public async Task StartSession_ShouldReturnCreatedAtAction()
    {
        var session = new Session { SessionId = 1, GameId = 1, PlayerName = "Player1", Duration = 60 };
        var dto = new StartSessionDto { GameId = 1, PlayerName = "Player1", Duration = 60 };

        _mockRepo.Setup(repo => repo.CreateSessionAsync(It.IsAny<Session>()))
            .ReturnsAsync(session);

        var result = await _controller.StartSession(dto) as CreatedAtActionResult;

        Assert.NotNull(result);
        Assert.Equal("GetSession", result!.ActionName);
        Assert.Equal(1, (result.Value as Session)!.SessionId);
    }

    [Fact]
    public async Task GetNextNumber_ShouldReturnOkResultWithNumber()
    {
        var game = new Game { GameId = 1, Author = "Player1", Min = 1, Max = 100, RuleSet = new Dictionary<int, string> { { 3, "Fizz" }, { 5, "Buzz" } } };
        var session = new Session { SessionId = 1, GameId = 1, Game = game, PlayerName = "Player1", Duration = 60 };
        var dto = new StartSessionDto { GameId = 1, PlayerName = "Player1", Duration = 60 };

        _mockRepo.Setup(repo => repo.CreateSessionAsync(It.IsAny<Session>()))
            .ReturnsAsync(session);

        _mockRepo.Setup(repo => repo.GetSessionByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(session);

        _mockRepo.Setup(repo => repo.GetUniqueRandomNumberAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(42);

        var startSessionResult = await _controller.StartSession(dto) as CreatedAtActionResult;
        var sessionId = (startSessionResult!.Value as Session)!.SessionId;
        var result = await _controller.GetNextNumber(sessionId) as OkObjectResult;

        Assert.NotNull(result);
        Assert.Equal(42, result!.Value);
    }

    [Fact]
    public async Task SubmitAnswer_ShouldReturnOkResultWithCorrectness()
    {
        var sessionId = 1;
        var number = 42;
        var answer = "CorrectAnswer";
        var dto = new SubmitAnswerDto { Number = number, Answer = answer };

        _mockRepo.Setup(repo => repo.SubmitAnswerAsync(sessionId, number, answer))
            .ReturnsAsync(true);

        var result = await _controller.SubmitAnswer(sessionId, dto) as OkObjectResult;

        Assert.NotNull(result);
        var value = result!.Value;
        Assert.True((bool)value?.GetType().GetProperty("isCorrect")!.GetValue(value, null)!);
    }

    [Fact]
    public async Task GetSession_ShouldReturnOkResultWithSession()
    {
        var session = new Session { SessionId = 1, GameId = 1, PlayerName = "Player1", Duration = 60 };

        _mockRepo.Setup(repo => repo.GetSessionByIdAsync(1))
            .ReturnsAsync(session);

        var result = await _controller.GetSession(1) as OkObjectResult;

        Assert.NotNull(result);
        Assert.Equal(session, result!.Value);
    }

    [Fact]
    public async Task GetSession_ShouldReturnNotFoundResult()
    {
        _mockRepo.Setup(repo => repo.GetSessionByIdAsync(1))
            .ReturnsAsync((Session?)null);

        var result = await _controller.GetSession(1) as NotFoundResult;

        Assert.NotNull(result);
    }
    [Fact]
    public async Task EndSession_ShouldReturnOkResult()
    {
        var session = new Session { SessionId = 1, GameId = 1, PlayerName = "Player1", Duration = 60, IsEnded = true };

        _mockRepo.Setup(repo => repo.EndSessionAsync(1))
            .ReturnsAsync(session);

        var result = await _controller.EndSession(1) as OkObjectResult;

        Assert.NotNull(result);
        Assert.Equal("Session ended successfully", result!.Value?.GetType().GetProperty("Message")?.GetValue(result.Value, null));
    }

    [Fact]
    public async Task EndSession_ShouldReturnNotFoundResult()
    {
        _mockRepo.Setup(repo => repo.EndSessionAsync(1))
            .ReturnsAsync((Session?)null);

        var result = await _controller.EndSession(1) as NotFoundObjectResult;

        Assert.NotNull(result);
        Assert.Equal("Session not found or already ended", result!.Value?.GetType().GetProperty("Message")?.GetValue(result.Value, null));
    }

    [Fact]
    public async Task GetSessionResults_ShouldReturnOkResultWithResults()
    {
        var results = new SessionResultDto
        {
            SessionId = 1,
            PlayerName = "Player1",
            TotalQuestions = 10,
            CorrectAnswers = 7,
            IncorrectAnswers = 3,
            FinalScore = 7
        };

        _mockRepo.Setup(repo => repo.GetSessionResultsAsync(1))
            .ReturnsAsync(results);

        var result = await _controller.GetSessionResults(1) as OkObjectResult;

        Assert.NotNull(result);
        Assert.Equal(results, result!.Value);
    }

    [Fact]
    public async Task GetSessionResults_ShouldReturnNotFoundResult()
    {
        _mockRepo.Setup(repo => repo.GetSessionResultsAsync(1))
            .ReturnsAsync((SessionResultDto?)null);

        var result = await _controller.GetSessionResults(1) as NotFoundObjectResult;

        Assert.NotNull(result);
        Assert.Equal("Session not found or not ended yet", result!.Value?.GetType().GetProperty("Message")?.GetValue(result.Value, null));
    }

}