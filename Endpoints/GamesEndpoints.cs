using Gamestore.Data;
using Gamestore.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Gamestore.Endpoints;

public static class GamesEndpoints  {
    // private static readonly List <GameSummaryDto> games = [
    //     new (
    //         1,
    //         "Street Fighter",
    //         "Fighting",
    //         19.99M,
    //         new DateOnly (1992, 7, 15)
    //     ),
    //     new (
    //         2,
    //         "Mario",
    //         "Adventure",
    //         19.99M,
    //         new DateOnly (1996, 9, 15)
    //     )
    // ];

    public static RouteGroupBuilder MapGamesEndpoints (this WebApplication app) {
        var group = app.MapGroup("games").WithParameterValidation();;

        // GET /games
        group.MapGet("/", (GameStoreContext dbContext) => 
            dbContext.Games
                    .Include(game => game.Genre)
                    .Select(game => game.ToGameSummaryDto())
                    .AsNoTracking());

        // GET /games/1
        group.MapGet ("/{id}", (int id, GameStoreContext dbContext) => {
            Game? game = dbContext.Games.Find(id);
            return game is null ? Results.NotFound() : Results.Ok(game.ToGameDetailsDto());

        }).WithName ("GetGame");

        // POST /games
        group.MapPost ("/", (CreateGameDto newGame, GameStoreContext dbContext) => {
            // GameDto game = new GameDto(
            //     games.Count + 1,
            //     newGame.Name, 
            //     newGame.Genre,
            //     newGame.Price, 
            //     newGame.ReleaseDate
            // );
            // games.Add (game);
            Game game = newGame.ToEntity();
            // Not necessary anymore
            // game.Genre = dbContext.Genres.Find(newGame.GenreId);

            dbContext.Games.Add(game);
            dbContext.SaveChanges();

            return Results.CreatedAtRoute(
                "GetGame", 
                new {id = game.Id}, 
                game.ToGameDetailsDto());
        });
        

        // PUT /games
        group.MapPut("/{id}", (int id, UpdateGameDto updateGame, GameStoreContext dbContext) => {
            var existingGame = dbContext.Games.Find(id);
            if (existingGame is null) {
                return Results.NotFound();
            }
            dbContext.Entry(existingGame)
                .CurrentValues.
                SetValues(updateGame.ToEntity(id));
            dbContext.SaveChanges();
            return Results.NoContent();
        });

        // DELETE /games/1
        group.MapDelete("/{id}", (int id, GameStoreContext dbContext) => {
            dbContext.Games
                    .Where(game => game.Id == id)
                    .ExecuteDelete();
            return Results.NoContent();
        });
        return group;
    }
}