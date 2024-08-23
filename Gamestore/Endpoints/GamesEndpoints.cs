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
        var group = app.MapGroup("games").WithParameterValidation();

        // GET /games
        group.MapGet("/", async (GameStoreContext dbContext) => 
            await dbContext.Games
                    .Include(game => game.Genre)
                    .Select(game => game.ToGameSummaryDto())
                    .AsNoTracking()
                    .ToListAsync());

        // GET /games/1
        group.MapGet ("/{id}", async (int id, GameStoreContext dbContext) => {
            // Load game using Find - quick find to get data from the cache, however it doesnt load related objects
            // Game? game = await dbContext.Games.FindAsync(id);
            // return game is null ? Results.NotFound() : Results.Ok(game.ToGameDetailsDto());

            // Using eager loading to find all the related objects. It finds directly from DB
            Game? game = await dbContext.Games.Include(game => game.Genre).FirstOrDefaultAsync(game => game.Id == id);
            return game is null ? Results.NotFound() : Results.Ok(game.ToGameSummaryDto());
        }).WithName ("GetGame");

        // POST /games
        group.MapPost ("/", async (CreateGameDto newGame, GameStoreContext dbContext) => {
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
            await dbContext.SaveChangesAsync();

            return Results.CreatedAtRoute(
                "GetGame", 
                new {id = game.Id}, 
                game.ToGameDetailsDto());
        });
        

        // PUT /games
        group.MapPut("/{id}", async (int id, UpdateGameDto updateGame, GameStoreContext dbContext) => {
            var existingGame = await dbContext.Games.FindAsync(id);
            if (existingGame is null) {
                return Results.NotFound();
            }
            dbContext.Entry(existingGame)
                .CurrentValues.
                SetValues(updateGame.ToEntity(id));
            await dbContext.SaveChangesAsync();
            return Results.NoContent();
        });

        // DELETE /games/1
        group.MapDelete("/{id}", async (int id, GameStoreContext dbContext) => {
            await dbContext.Games
                    .Where(game => game.Id == id)
                    .ExecuteDeleteAsync();
            return Results.NoContent();
        });
        return group;
    }
}