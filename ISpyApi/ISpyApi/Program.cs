using ISpyApi;
using ISpyApi.Utilities;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Create service and allow service to be injectable
builder.Services.AddSingleton<ISpyApiService>();
builder.Services.AddHostedService(p => p.GetRequiredService<ISpyApiService>());

var app = builder.Build();

app.MapGet("/host/{hostname}", (string hostname, ISpyApiService service) =>
{
    if (!Verify.Username(ref hostname))
    {
        return "error";
    }

    var response = service.Host(hostname);
    return Schemas.ToJson(response);
});

app.MapGet("/join/{code}/{username}", (ulong code, string username, ISpyApiService service) =>
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

app.MapPost("/poll/{guid}", async (Guid guid, HttpRequest request, Stream body, ISpyApiService service) =>
{
    if (request.ContentLength is not null && request.ContentLength > ISpyApiService.MaxMessageSize)
    {
        return Results.BadRequest();
    }

    var readSize = (int?)request.ContentLength ?? (ISpyApiService.MaxMessageSize + 1);
    var buffer = new byte[readSize];
    var read = await body.ReadAtLeastAsync(buffer, readSize, false);

    if (read > ISpyApiService.MaxMessageSize)
    {
        return Results.BadRequest();
    }

    try
    {
        string data = Encoding.UTF8.GetString(buffer);
        Console.WriteLine(data);
        Console.WriteLine("------------------------");
        data = Uri.UnescapeDataString(data);
        string[] lines = data.Split('\n', StringSplitOptions.TrimEntries);

        List<object> schemas = new();
        for (int i = 0; i < lines.Length / 2; i += 2)
        {
            string name = lines[i];
            string json = lines[i + 1];
            if (Schemas.FromJson(name, json, out object? schema))
            {
                schemas.Add(schema!);
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

app.Run();