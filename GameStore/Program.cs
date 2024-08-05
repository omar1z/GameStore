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

app.MapGet("/", () => "Hello World!");
app.MapGet("/hi", () => "hi man");

app.Run();
