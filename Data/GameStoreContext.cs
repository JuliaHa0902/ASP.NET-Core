using Gamestore.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gamestore.Data;

// public class GameStoreContext(DbContextOptions<GameStoreContext> options) : DbContext(options) {
//     public DbSet<Game> Games => Set<Game>();
//     public DbSet<Genre> Genres => Set<Genre>();
// }
public class GameStoreContext : DbContext {
    public GameStoreContext(DbContextOptions<GameStoreContext> options): base(options) { }
    public DbSet<Game> Games => Set<Game>();
    public DbSet<Genre> Genres => Set<Genre>();
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Genre>().HasData (
            new { Id = 1, Name = "Fighting"},
            new { Id = 2, Name = "Roleplaying"},
            new { Id = 3, Name = "Sports"},
            new { Id = 4, Name = "Sports"},
            new { Id = 5, Name = "Racing" }, 
            new { Id = 6, Name = "Kids and Family" } 
        );
    }
}