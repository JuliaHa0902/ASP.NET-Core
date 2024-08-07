using Gamestore.Data;
using Gamestore.Dtos;
using Gamestore.Endpoints;
var builder = WebApplication.CreateBuilder(args);

var connString = builder.Configuration.GetConnectionString ("GameStore");
builder.Services.AddSqlite<GameStoreContext>(connString);

var app = builder.Build();
app.MapGamesEndpoints();
app.MapGenresEndpoints();
await app.MigrateDbAsync();
app.Run();
