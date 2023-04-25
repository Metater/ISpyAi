using ISpyApi;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

object gamesLock = new();
Games games = new();
StringBuilder gamesSb = new();

app.MapGet("/host", (string hostname, float aiPercentage) =>
{
    if (!Verify.Username(ref hostname))
    {
        return "error: hostname";
    }

    if (!Verify.AiPercentage(aiPercentage))
    {
        return "error: ai percentage";
    }

    lock (gamesLock)
    {
        var game = games.Host(hostname, aiPercentage);
        gamesSb.Clear();
        gamesSb.AppendLine(game.Host.Guid.ToString());
        gamesSb.AppendLine(game.Code.ToString());
        return gamesSb.ToString();
    }
});

app.MapGet("/join", (ulong code, string username) =>
{
    if (!Verify.Username(ref username))
    {
        return "error: username";
    }

    lock (gamesLock)
    {
        if (games.Join(code, username, out Player? player))
        {
            gamesSb.Clear();
            gamesSb.AppendLine(player!.Guid.ToString());
            return gamesSb.ToString();
        }

        return "error: joining game";
    }
});

app.MapGet("/players", (Guid guid) =>
{
    lock (gamesLock)
    {
        if (games.GetGameWithGuid(guid, out Game? game))
        {
            return JsonSerializer.Serialize(game!.Players);
        }

        return "error: getting game with guid";
    }
});

app.Run();