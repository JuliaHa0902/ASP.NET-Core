using GameStore.Frontend.Models;

namespace GameStore.Frontend.Clients;

public class GamesClient {
    private readonly List<GameSummary> games = [
        new() {
            Id = 1,
            Name = "Spy x Family",
            Genre = "Fighting",
            Price = 19.99M,
            ReleaseDate = new DateOnly(2023, 1, 3)
        },
        new() {
            Id = 2,
            Name = "Final Fantasy",
            Genre = "Role Playing",
            Price = 19.99M,
            ReleaseDate = new DateOnly(2023, 1, 3)
        },
        new() {
            Id = 1,
            Name = "Mario",
            Genre = "Fighting",
            Price = 19.99M,
            ReleaseDate = new DateOnly(2023, 1, 3)
        }
    ];

    public GameSummary[] GetGames() => [..games];
    private readonly Genre[] genres = new GenresClient().GetGenres();
    public void AddGame(GameDetails game) {
        ArgumentException.ThrowIfNullOrWhiteSpace(game.GenreId);
        var genre = genres.Single(genre => genre.Id == int.Parse(game.GenreId));
        var gameSummary = new GameSummary {
            Id = games.Count + 1,
            Name = game.Name,
            Genre = genre.Name,
            Price = game.Price,
            ReleaseDate = game.ReleaseDate 
        };
        Console.WriteLine(game.Name);
        games.Add(gameSummary);
    }
}