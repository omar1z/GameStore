using GameStore.Dtos;

namespace GameStore.Endpoints
{
    public static class GamesEndpoints
    {
        private static List<GameDto> games = [
    new (
        1,
        "CS",
        "War",
        12.99M,
        new DateOnly(2001, 8, 20)
    ),
    new (
        2,
        "FIFA",
        "Sports",
        20.99M,
        new DateOnly(2022, 8, 20)
    ),
    new (
        3,
        "Rocket League",
        "Car Sports",
        33.99M,
        new DateOnly(2022, 4, 11)
    )
];

        public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("games")
                .WithParameterValidation();// to notice the data annotation that was required in the createGameDto

            // GET /games
            group.MapGet("/", () => games);// minimal api

            //GET /games/1
            group.MapGet("/{id}", (int id) => {
                GameDto? game = games.Find(game => game.Id == id);
                return game is null ? Results.NotFound() : Results.Ok(game);
            })
                .WithName("GetGame");

            // POST /games
            group.MapPost("/", (CreateGameDto newGame) =>
            {

                GameDto game = new(
                    games.Count + 1,
                    newGame.Name,
                    newGame.Genre,
                    newGame.Price,
                    newGame.RealeaseDate
                    );

                games.Add(game);

                return Results.CreatedAtRoute("GetGame", new { id = game.Id }, game);
            });

            // PUT /games
            group.MapPut("/{id}", (int id, UpdateGameDto updateGame) =>
            {
                var index = games.FindIndex(game => game.Id == id);
                if (index == -1)
                {
                    return Results.NotFound();
                }

                games[index] = new GameDto(
                    id,
                    updateGame.Name,
                    updateGame.Genre,
                    updateGame.Price,
                    updateGame.RealeaseDate
                    );

                return Results.NoContent();
            });

            // DELETE /games/1
            group.MapDelete("/{id}", (int id) =>
            {
                games.RemoveAll(game => game.Id == id);
                return Results.NoContent();
            });

            return group;
        }
    }
}
