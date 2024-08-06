using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Data
{
    public static class DataExtensions
    {
        public static async Task MigrateDbAsync(this WebApplication app)
        {
            // we cannot access DbContext directly we need a scope to interact with database
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<GameStoreContext>();
            await dbContext.Database.MigrateAsync();
        }
    }
}
