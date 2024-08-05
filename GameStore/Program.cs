using GameStore.Dtos;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

List<GameDto> games = [
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

// GET /games
app.MapGet("games", () => games);// minimal api

//GET /games/1
app.MapGet("games/{id}", (int id) => {
    GameDto? game = games.Find(game => game.Id == id);
    return game is null ? Results.NotFound() : Results.Ok(game);
})
    .WithName("GetGame");

// POST /games
app.MapPost("games", (CreateGameDto newGame) =>
{
    GameDto game = new(
        games.Count+1,
        newGame.Name,
        newGame.Genre,
        newGame.Price,
        newGame.RealeaseDate
        );

    games.Add(game);

    return Results.CreatedAtRoute("GetGame", new { id = game.Id}, game);
});

// PUT /games
app.MapPut("games/{id}", (int id, UpdateGameDto updateGame) =>
{
    var index = games.FindIndex(game => game.Id == id);
    if(index == -1)
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
app.MapDelete("games/{id}", (int id) =>
{
    games.RemoveAll(game => game.Id == id);
    return Results.NoContent();
});

app.Run();
