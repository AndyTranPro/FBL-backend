using System;
using FooBooLooGameAPI.DTOs;
using FooBooLooGameAPI.Entities;

namespace FooBooLooGameAPI.Repositories.Interfaces;

public interface ISessionRepository
{
  Task<Session> CreateSessionAsync(Session session);
  Task<Session> GetSessionByIdAsync(int sessionId);
  Task<int> GetUniqueRandomNumberAsync(int sessionId, int min, int max);
  Task<bool> SubmitAnswerAsync(int sessionId, int number, string answer);
  Task<Session> EndSessionAsync(int sessionId);
  Task<SessionResultDto> GetSessionResultsAsync(int sessionId);
}
