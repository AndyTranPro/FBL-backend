using System;
using FooBooLooGameAPI.Data;
using FooBooLooGameAPI.Entities;
using FooBooLooGameAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FooBooLooGameAPI.Repositories.Implementations;

public class GameRepository : IGameRepository
{
    private readonly GameDbContext _context;

    public GameRepository(GameDbContext context)
    {
        _context = context;
    }

    public async Task<Game> CreateGameAsync(Game game)
    {
        // check if the game already exists
        var existingGame = await _context.Games.FirstOrDefaultAsync(g => g.Name == game.Name);
        if (existingGame != null)
        {
            return null;
        }
        _context.Games.Add(game);
        await _context.SaveChangesAsync();
        return game;
    }

    public async Task<IEnumerable<Game>> GetGamesAsync()
    {
        return await _context.Games.ToListAsync();
    }

    public async Task<Game> GetGameByIdAsync(int gameId)
    {
        return await _context.Games.FindAsync(gameId);
    }
}
