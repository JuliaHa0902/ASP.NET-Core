namespace Gamestore.Dtos;

public record class GameDetailsDto (int Id, string Name, int GenreId, decimal Price, DateOnly ReleaseDate);