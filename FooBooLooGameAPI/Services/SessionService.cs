using System;
using FooBooLooGameAPI.DTOs;
using FooBooLooGameAPI.Entities;
using FooBooLooGameAPI.Repositories.Interfaces;

namespace FooBooLooGameAPI.Services;

public class SessionService
{
    private readonly ISessionRepository _sessionRepository;

    public SessionService(ISessionRepository sessionRepository)
    {
        _sessionRepository = sessionRepository;
    }

    public async Task<Session> StartSessionAsync(int gameId, string playerName, int duration)
    {
        var session = new Session { GameId = gameId, PlayerName = playerName, Duration = duration };
        return await _sessionRepository.CreateSessionAsync(session);
    }

    public async Task<Session> GetSessionAsync(int sessionId)
    {
        return await _sessionRepository.GetSessionByIdAsync(sessionId);
    }

    public async Task<int> GetNextRandomNumberAsync(int sessionId, int min, int max)
    {
        return await _sessionRepository.GetUniqueRandomNumberAsync(sessionId, min, max);
    }

    public async Task<bool> SubmitAnswerAsync(int sessionId, int number, string answer)
    {
        return await _sessionRepository.SubmitAnswerAsync(sessionId, number, answer);
    }
    public async Task<Session> EndSessionAsync(int sessionId)
    {
        return await _sessionRepository.EndSessionAsync(sessionId);
    }

    public async Task<SessionResultDto> GetSessionResultsAsync(int sessionId)
    {
        return await _sessionRepository.GetSessionResultsAsync(sessionId);
    }
}
