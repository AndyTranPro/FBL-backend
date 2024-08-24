using System;
using FooBooLooGameAPI.Entities;
using FooBooLooGameAPI.Repositories.Interfaces;

namespace FooBooLooGameAPI.Services;

public class GameService
{
  private readonly IGameRepository _gameRepository;

  public GameService(IGameRepository gameRepository)
  {
      _gameRepository = gameRepository;
  }

  public async Task<Game> CreateGameAsync(string name, string author, int min, int max, Dictionary<int, string> ruleSet)
  {
      var game = new Game { Name = name, Author = author, Min = min, Max = max, RuleSet = ruleSet };
      return await _gameRepository.CreateGameAsync(game);
  }

    public async Task<Game> GetGameAsync(int id)
    {
        return await _gameRepository.GetGameByIdAsync(id);
    }

  public async Task<IEnumerable<Game>> GetGamesAsync()
  {
      return await _gameRepository.GetGamesAsync();
  }
}
