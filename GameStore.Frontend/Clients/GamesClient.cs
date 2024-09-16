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
            Id = 3,
            Name = "Mario",
            Genre = "Fighting",
            Price = 19.99M,
            ReleaseDate = new DateOnly(2023, 1, 3)
        }
    ];

    public GameSummary[] GetGames() => [..games];
    private readonly Genre[] genres = new GenresClient().GetGenres();
    public void AddGame(GameDetails game) {
        Genre genre = GetGenreById(game.GenreId);
        var gameSummary = new GameSummary {
            Id = games.Count + 1,
            Name = game.Name,
            Genre = genre.Name,
            Price = game.Price,
            ReleaseDate = game.ReleaseDate 
        };
        // Console.WriteLine(game.Name);
        games.Add(gameSummary);
    }

    public GameDetails GetGame(int id) {
        GameSummary game = GetGameSummaryById(id);

        var genre = genres.Single(genre => string.Equals(
            genre.Name,
            game.Genre,
            StringComparison.OrdinalIgnoreCase
        ));

        return new GameDetails {
            Id = game.Id,
            Name = game.Name,
            GenreId = genre.Id.ToString(),
            Price = game.Price,
            ReleaseDate = game.ReleaseDate
        };
    }

    public void UpdateGame(GameDetails updateGame) {
        var genre = GetGenreById(updateGame.GenreId);
        GameSummary existingGame = GetGameSummaryById(updateGame.Id);

        existingGame.Name = updateGame.Name;
        existingGame.Genre = genre.Name;
        existingGame.Price = updateGame.Price;
        existingGame.ReleaseDate = updateGame.ReleaseDate;
    }

    public void DeleteGame (int id) {
        var game = GetGameSummaryById(id);
        games.Remove(game);
    }

    private Genre GetGenreById(string? id) {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        return  genres.Single(genre => genre.Id == int.Parse(id));
    }

    private GameSummary GetGameSummaryById(int id) {
        GameSummary? game = games.Find(game => game.Id == id);
        ArgumentNullException.ThrowIfNull(game);
        return game;
    }
}