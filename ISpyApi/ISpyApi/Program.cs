using ISpyApi;
using ISpyApi.Utilities;
using System.Text;

// Create app
var builder = WebApplication.CreateBuilder(args);

// Create service and allow service to be injectable
builder.Services.AddSingleton<GameService>();
builder.Services.AddHostedService(p => p.GetRequiredService<GameService>());

// Build app
var app = builder.Build();

// Called by a unity client that wants to host a game
app.MapGet("/host/{hostname}", (string hostname, GameService service) =>
{
    if (!Verify.Username(ref hostname))
    {
        return "error";
    }

    var response = service.Host(hostname);
    return Schemas.ToJson(response);
});

// Called by a unity client that wants to join a game
app.MapGet("/join/{code}/{username}", (ulong code, string username, GameService service) =>
{
    if (!Verify.Username(ref username))
    {
        return "error";
    }

    if (service.Join(code, username, out var response))
    {
        return Schemas.ToJson(response!);
    }

    return "error";
});

// Called by unity clients perodically
app.MapPost("/poll/{guid}", async (Guid guid, HttpRequest request, Stream body, GameService service) =>
{
    if (request.ContentLength is not null && request.ContentLength > GameService.MaxMessageSize)
    {
        return Results.BadRequest();
    }

    var readSize = (int?)request.ContentLength ?? (GameService.MaxMessageSize + 1);
    var buffer = new byte[readSize];
    var read = await body.ReadAtLeastAsync(buffer, readSize, false);

    if (read > GameService.MaxMessageSize)
    {
        return Results.BadRequest();
    }

    try
    {
        string data = Encoding.UTF8.GetString(buffer);
        data = Uri.UnescapeDataString(data);
        string[] lines = data.Split('\n', StringSplitOptions.TrimEntries);

        List<object> schemas = new();
        for (int i = 0; i < lines.Length / 2; i += 2)
        {
            string name = lines[i];
            string json = lines[i + 1];

            if (Schemas.FromJson(name, json, out object schema))
            {
                schemas.Add(schema);
            }
            else
            {
                return Results.BadRequest();
            }
        }

        if (!service.Input(guid, schemas))
        {
            return Results.StatusCode(StatusCodes.Status429TooManyRequests);
        }

        await Task.Delay(100);

        string output = service.Output(guid);
        return Results.Text(output);
    }
    catch
    {
        return Results.BadRequest();
    }
});

app.Run("http://*:44464");