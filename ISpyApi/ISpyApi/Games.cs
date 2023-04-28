using ISpyApi.General;
using ISpyApi.Utilities;

namespace ISpyApi;

public class Games
{
    private readonly Resources resources;
    private readonly List<Game> games = new();

    public Games(Action<Guid, object> sendSchema)
    {
        Random random = new();
        resources = new Resources(random, new(random), new(random), sendSchema);
    }

    public Game Host(string hostname)
    {
        Game game = new(resources, hostname);
        games.Add(game);
        return game;
    }

    public bool Join(ulong code, string username, out Player? player)
    {
        var game = games.Find(g => g.Code == code);
        if (game is null)
        {
            player = default;
            return false;
        }

        player = game.Join(username);
        return true;
    }

    public void RequestPeriodicOutput(Guid guid)
    {
        if (GetGameWithGuid(guid, out var game))
        {
            resources.SendSchema(guid, new PeriodicUpdate
            {
                serverFileTimeUtc = DateTime.UtcNow.ToFileTimeUtc(),
                players = game!.Players.Values.Select(p => p.Username).ToList()
            });
        }
    }

    public void SchemaReceived(Guid guid, object schema)
    {
        {
            Console.WriteLine($"Got unimplemented schema type: {schema}");
        }
    }

    public void Tick(double deltaTime)
    {
        games.ForEach(g => g.Tick(deltaTime));
    }

    private bool GetGameWithGuid(Guid guid, out Game? game)
    {
        game = games.Find(g => g.Players.ContainsKey(guid));
        return game is not null;
    }
}