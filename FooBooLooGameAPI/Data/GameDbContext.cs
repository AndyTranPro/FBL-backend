using System;
using FooBooLooGameAPI.Entities;
using FooBooLooGameAPI.Helpers;
using Microsoft.EntityFrameworkCore;

namespace FooBooLooGameAPI.Data;

public class GameDbContext : DbContext
{
    public GameDbContext(DbContextOptions<GameDbContext> options) : base(options)
    {
    }

    public DbSet<Game> Games { get; set; } = null!;
    public DbSet<Session> Sessions { get; set; } = null!;
    public DbSet<SessionNumber> SessionNumbers { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var dictionaryToJsonConverter = new DictionaryToJsonConverter();
        var dictionaryValueComparer = new DictionaryValueComparer();

        modelBuilder.Entity<Game>()
            .Property(g => g.RuleSet)
            .HasConversion(dictionaryToJsonConverter)
            .HasColumnType("jsonb")
            .Metadata.SetValueComparer(dictionaryValueComparer);

        modelBuilder.Entity<Session>()
            .HasIndex(s => new { s.GameId, s.PlayerName, s.StartTime })
            .IsUnique();
    }
}
