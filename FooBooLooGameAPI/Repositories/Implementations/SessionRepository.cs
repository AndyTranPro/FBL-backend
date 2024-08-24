using System;
using FooBooLooGameAPI.Data;
using FooBooLooGameAPI.DTOs;
using FooBooLooGameAPI.Entities;
using FooBooLooGameAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FooBooLooGameAPI.Repositories.Implementations;

public class SessionRepository : ISessionRepository
{
    private readonly GameDbContext _context;

    public SessionRepository(GameDbContext context)
    {
        _context = context;
    }

    public async Task<Session> CreateSessionAsync(Session session)
    {
        _context.Sessions.Add(session);
        await _context.SaveChangesAsync();
        return session;
    }

    public async Task<Session> GetSessionByIdAsync(int sessionId)
    {
        return await _context.Sessions
            .Include(s => s.Game)
            .Include(s => s.SessionNumbers)
            .FirstOrDefaultAsync(s => s.SessionId == sessionId);
    }

    public async Task<int> GetUniqueRandomNumberAsync(int sessionId, int min, int max)
    {
        var usedNumbers = await _context.SessionNumbers
            .Where(sn => sn.SessionId == sessionId)
            .Select(sn => sn.NumberServed)
            .ToListAsync();

        Random rnd = new();
        int newNumber;
        do
        {
            newNumber = rnd.Next(min, max + 1);
        } while (usedNumbers.Contains(newNumber));

        return newNumber;
    }

    public async Task<bool> SubmitAnswerAsync(int sessionId, int number, string answer)
    {
        var session = await GetSessionByIdAsync(sessionId);
        if (session == null) return false;

        var expectedAnswer = GenerateExpectedAnswer(session.Game.RuleSet, number);
        var isCorrect = expectedAnswer == answer;

        _context.SessionNumbers.Add(new SessionNumber
        {
            SessionId = sessionId,
            NumberServed = number,
            IsCorrect = isCorrect
        });

        if (isCorrect) session.Score++;
        await _context.SaveChangesAsync();
        return isCorrect;
    }

    private static string GenerateExpectedAnswer(Dictionary<int, string> rules, int number)
    {
        return string.Concat(rules
            .Where(rule => number % rule.Key == 0)
            .Select(rule => rule.Value));
    }

    public async Task<Session> EndSessionAsync(int sessionId)
    {
        var session = await GetSessionByIdAsync(sessionId);
        if (session == null || session.IsEnded) return null;

        session.IsEnded = true;
        await _context.SaveChangesAsync();
        return session;
    }

    public async Task<SessionResultDto> GetSessionResultsAsync(int sessionId)
    {
        var session = await GetSessionByIdAsync(sessionId);
        if (session == null || !session.IsEnded) return null;

        var totalQuestions = await _context.SessionNumbers
            .CountAsync(sn => sn.SessionId == sessionId);

        var correctAnswers = await _context.SessionNumbers
            .CountAsync(sn => sn.SessionId == sessionId && sn.IsCorrect);

        return new SessionResultDto
        {
            SessionId = session.SessionId,
            PlayerName = session.PlayerName,
            TotalQuestions = totalQuestions,
            CorrectAnswers = correctAnswers,
            IncorrectAnswers = totalQuestions - correctAnswers,
            FinalScore = session.Score
        };
    }
}
