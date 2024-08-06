using Microsoft.EntityFrameworkCore;

namespace Gamestore.Data;

public static class DataExtension
{
    public static void MigrateDb(this WebApplication app) {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<GameStoreContext>();
        dbContext.Database.Migrate();
    }
}
