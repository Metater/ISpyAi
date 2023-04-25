namespace ISpyApi;

public class Games
{
    private readonly Random random = new();
    private readonly ImageFactory imageFactory;
    private readonly CodeFactory codeFactory = new();
    private readonly List<Game> games = new();

    public Games()
    {
        imageFactory = new(random);
    }

    public Game Host(string hostname, float aiPercentage)
    {
        Game game = new(random, imageFactory, codeFactory, hostname, aiPercentage);
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
}