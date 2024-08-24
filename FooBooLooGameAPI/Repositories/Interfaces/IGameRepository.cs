using System;
using FooBooLooGameAPI.Entities;

namespace FooBooLooGameAPI.Repositories.Interfaces;

public interface IGameRepository
{
  Task<Game> CreateGameAsync(Game game);
  Task<IEnumerable<Game>> GetGamesAsync();
  Task<Game> GetGameByIdAsync(int gameId);
}
