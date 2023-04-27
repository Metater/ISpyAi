using ISpyApi.Factories;
using ISpyApi.Utilities;

namespace ISpyApi;

public class Games
{
    private readonly Action<Guid, object> send;
    private readonly Random random = new();
    private readonly ImageFactory imageFactory;
    private readonly CodeFactory codeFactory = new();
    private readonly List<Game> games = new();

    public Games(Action<Guid, object> send)
    {
        this.send = send;
        imageFactory = new(random);
    }

    public Game Host(string hostname)
    {
        Game game = new(random, imageFactory, codeFactory, hostname);
        game.Init();
        games.Add(game);
        return game;
    }

    public bool Join(ulong code, string username, out Player? player)
    {
        var game = games.Find(g => g.Code == code);
        if (game is null)
        {
            player = null;
            return false;
        }

        player = game.Join(username);
        return true;
    }

    public List<Player> GetPlayersInGame(Guid guid)
    {
        if (GetGameWithGuid(guid, out Game? game))
        {
            return game!.Players;
        }

        return new();
    }

    public bool GetGameWithGuid(Guid guid, out Game? game)
    {
        game = games.Find(g => g.HasPlayerWithGuid(guid));
        return game is not null;
    }

    public void RequestPeriodicOutput(Guid guid)
    {
        if (GetGameWithGuid(guid, out var game))
        {
            send(guid, new PlayersResponse
            {
                players = game!.Players.Select(p => p.Username).ToList()
            });
        }
    }

    public void HandleSchema(Guid guid, object schema)
    {
        if (schema is Guid test)
        {

        }
        else
        {
            Console.WriteLine($"Got unimplemented schema type: {schema}");
        }
    }
}