using GameStore.Data;
using GameStore.Dtos;
using GameStore.Entities;
using GameStore.Mapping;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Endpoints
{
    public static class GamesEndpoints
    {
        public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("games")
                .WithParameterValidation();// to notice the data annotation that was required in the createGameDto

            // GET /games
            group.MapGet("/", async (GameStoreContext dbContext) => 
            await dbContext.Games
                .Include(game => game.Genre)
                .Select(game => game.ToGameSummaryDto())
                .AsNoTracking()
                .ToListAsync());

            //GET /games/1
            group.MapGet("/{id}", async (int id, GameStoreContext dbContext) => 
            {
                Game? game = await dbContext.Games.FindAsync(id);

                return game is null ? Results.NotFound() : Results.Ok(game.ToGameDetailsDto());
            })
                .WithName("GetGame");

            // POST /games
            group.MapPost("/", async (CreateGameDto newGame, GameStoreContext dbContext) =>
            {

                Game game = newGame.ToEntity();

                dbContext.Games.Add(game);
                await dbContext.SaveChangesAsync();

                return Results.CreatedAtRoute("GetGame", new { id = game.Id }, game.ToGameDetailsDto());
            });

            // PUT /games
            group.MapPut("/{id}", async (int id, UpdateGameDto updateGame, GameStoreContext dbContext) =>
            {
                var existingGame = await dbContext.Games.FindAsync(id);

                if (existingGame is null)
                {
                    return Results.NotFound();
                }

                dbContext.Entry(existingGame).CurrentValues.SetValues(updateGame.ToEntity(id));

                await dbContext.SaveChangesAsync();

                return Results.NoContent();
            });

            // DELETE /games/1
            group.MapDelete("/{id}", async (int id, GameStoreContext dbContext) =>
            {
                await dbContext.Games.Where(game => game.Id == id).ExecuteDeleteAsync();
                return Results.NoContent();
            });

            return group;
        }
    }
}
