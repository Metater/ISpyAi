using ISpyApi;
using ISpyApi.Utilities;
using System.Text;
using System.Threading.Channels;

const int MaxMemory = 500 * 1024 * 1024;
const int MaxMessageSize = 80 * 1024;
const int MaxChannelSize = MaxMemory / MaxMessageSize;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton((_) => Channel.CreateBounded<(object schema, GuidBox? guidBox)>(MaxChannelSize));
builder.Services.AddHostedService<ISpyApiService>();

var app = builder.Build();

app.MapPost("/poll", async (HttpRequest request, Stream body, Channel<(object schema, GuidBox? guidBox)> channel) =>
{
    if (request.ContentLength is not null && request.ContentLength > MaxMessageSize)
    {
        return Results.BadRequest();
    }

    var readSize = (int?)request.ContentLength ?? (MaxMessageSize + 1);
    var buffer = new byte[readSize];
    var read = await body.ReadAtLeastAsync(buffer, readSize, false);

    if (read > MaxMessageSize)
    {
        return Results.BadRequest();
    }

    try
    {
        string data = Encoding.UTF8.GetString(buffer);
        data = Uri.UnescapeDataString(data);
        string[] lines = data.Split('\n', StringSplitOptions.TrimEntries);

        Guid guid = Guid.Empty;
        bool hasGuid = false;

        GuidBox? guidBox = null;

        if (lines.Length > 0 && Guid.TryParse(lines[0], out guid))
        {
            hasGuid = true;
        }
        else
        {
            guidBox = new();
        }

        for (int i = hasGuid ? 1 : 0; i < lines.Length / 2; i += 2)
        {
            string name = lines[i];
            string json = lines[i + 1];
            if (Schemas.FromJson(name, json, out object? schema))
            {
                if (!channel.Writer.TryWrite((schema!, guidBox)))
                {
                    return Results.StatusCode(StatusCodes.Status429TooManyRequests);
                }
            }
            else
            {
                return Results.BadRequest();
            }
        }

        await Task.Delay(100);

        if (hasGuid)
        {
            // TODO Read then Results.Text()
            return Results.Accepted();
        }
        else
        {
            guid = guidBox!.Get();
            return Results.Text(guid.ToString());
        }
    }
    catch
    {
        return Results.BadRequest();
    }
});



/*app.MapGet("/host", (string hostname, float aiPercentage) =>
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
});*/

app.Run();