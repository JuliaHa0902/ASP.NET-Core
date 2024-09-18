using GameStore.Frontend.Models;

namespace GameStore.Frontend.Clients;

public class GamesClient(HttpClient httpClient) {
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

    public async Task<GameSummary[]> GetGamesAsync() 
        => await httpClient.GetFromJsonAsync<GameSummary[]>("games") ?? [];

    public async Task AddGameAsync(GameDetails game) 
        => await httpClient.PostAsJsonAsync("games", game); 

    public async Task<GameDetails> GetGameAsync(int id)
        => await httpClient.GetFromJsonAsync<GameDetails>($"games/{id}")
            ?? throw new Exception ("Could not find game!");

    public async Task UpdateGameAsync(GameDetails updateGame)
        => await httpClient.PutAsJsonAsync($"games/{updateGame.Id}", updateGame);

    public async Task DeleteGameAsync (int id) 
        => await httpClient.DeleteAsync($"games/{id}");

}