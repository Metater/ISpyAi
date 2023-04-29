using ISpyApi;
using ISpyApi.Utilities;
using System.Diagnostics;
using System.Text;

const int ServiceProcessWaitMs = 100;
const int TickPeriodMs = 100;

// Create app builder
var builder = WebApplication.CreateBuilder(args);

// Create hosted service and allow service to be injectable
builder.Services.AddSingleton<GameService>();
builder.Services.AddHostedService(p => p.GetRequiredService<GameService>());

// Build app
var app = builder.Build();

// Called by a unity client that wants to host a game
app.MapGet("/host/{gameType}/{hostname}", (string gameType, string hostname, GameService service) =>
{
    if (!Verify.Username(ref hostname))
    {
        return "error";
    }

    if (service.Host(gameType, hostname, out var response))
    {
        return Schemas.ToJson(response);
    }

    return "error";
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
        return Schemas.ToJson(response);
    }

    return "error";
});

// Called by unity clients perodically to send and receive data
app.MapPost("/poll/{guid}", async (Guid guid, HttpRequest request, Stream body, GameService service) =>
{
    // Reject if client is sending too much data
    if (request.ContentLength is not null && request.ContentLength > GameService.MaxMessageSize)
    {
        return Results.BadRequest();
    }

    // Read data into byte array
    var readSize = (int?)request.ContentLength ?? (GameService.MaxMessageSize + 1);
    var buffer = new byte[readSize];
    var read = await body.ReadAtLeastAsync(buffer, readSize, false);

    // Disallow client from sending too much data
    if (read > GameService.MaxMessageSize)
    {
        return Results.BadRequest();
    }

    try
    {
        // Convert byte array into string and split lines
        string data = Encoding.UTF8.GetString(buffer);
        data = Uri.UnescapeDataString(data);
        string[] lines = data.Split('\n', StringSplitOptions.TrimEntries);

        // Deserialize schemas
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

        // Send schemas to service
        if (!service.HandleSchemas(guid, schemas))
        {
            return Results.StatusCode(StatusCodes.Status429TooManyRequests);
        }

        // Wait for service to process schemas and queue responses
        await Task.Delay(ServiceProcessWaitMs);

        // Send back any queued schemas from service
        string output = service.RequestSchemas(guid);
        return Results.Text(output);
    }
    catch
    {
        return Results.BadRequest();
    }
});

// Run app on port 44464
_ = app.RunAsync("http://*:44464");

// Tick service periodically
GameService service = app.Services.GetRequiredService<GameService>();
Stopwatch sw = Stopwatch.StartNew();
double lastSeconds = 0;
while (!Console.KeyAvailable)
{
    await Task.Delay(TickPeriodMs);

    double seconds = sw.Elapsed.TotalSeconds;
    double deltaTime = seconds - lastSeconds;
    lastSeconds = seconds;

    service.Tick(deltaTime);
}