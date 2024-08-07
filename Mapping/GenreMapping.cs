using Gamestore.Dtos;
using Gamestore.Entities;

namespace Gamestore.Mapping;

public static class GenreMapping
{
    public static GenreDto ToDto(this Genre genre) {
        return new GenreDto(genre.Id, genre.Name);
    }
}
