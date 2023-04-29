using ISpyApi.Fibbage;
using ISpyApi.Interfaces;
using ISpyApi.Models;

namespace ISpyApi;

public class Games : ITickable
{
    private const double GameTimeoutSeconds = 5;

    private readonly Resources resources;
    private readonly List<Game> games = new();

    public Games(Action<Guid, object> sendSchema)
    {
        Random random = new();
        resources = new Resources(random, new(random), new(random), sendSchema);
    }

    public bool Host(string gameType, string hostname, out Game? game)
    {
        game = gameType switch
        {
            FibbageGame.GameType => new FibbageGame(resources, hostname),
            _ => null
        };

        if (game is not null)
        {
            games.Add(game);
            return true;
        }

        return false;
    }

    public bool Join(ulong code, string username, out Player? player)
    {
        player = null;

        var game = games.Find(g => g.Code == code);
        if (game is not null)
        {
            player = game.Join(username);
        }

        return player is not null;
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

    public void HandleSchemas(Guid guid, List<object> schemas)
    {
        if (GetGameWithGuid(guid, out var game))
        {
            foreach (var schema in schemas)
            {
                if (!game!.HandleSchema(guid, schema))
                {
                    Console.WriteLine($"Schema not handled: {schema}");
                }
            }
        }
    }

    public void Tick(double deltaTime)
    {
        games.RemoveAll(g => (g as ITimeout).ShouldTimeout(GameTimeoutSeconds));

        games.ForEach(g => g.Tick(deltaTime));
    }

    private bool GetGameWithGuid(Guid guid, out Game? game)
    {
        game = games.Find(g => g.Players.ContainsKey(guid));
        return game is not null;
    }
}