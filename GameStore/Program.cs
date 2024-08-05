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
app.MapGet("games/{id}", (int id) => games.Find(game => game.Id == id)).WithName("GetGame");

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

app.Run();
