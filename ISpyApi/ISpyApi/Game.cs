using ISpyApi.Factories;

namespace ISpyApi;

public class Game
{
    private const double TimeoutSeconds = 10;

    private readonly Resources resources;
    public Player Host { get; init; }
    public ulong Code { get; init; }
    public Dictionary<Guid, Player> Players { get; init; } = new();
    public DateTime LastUsedTime { get; private set; } = DateTime.Now;

    public Game(Resources resources, string hostname)
    {
        this.resources = resources;
        Host = new(hostname);
        Code = resources.CodeFactory.GetCode();

        Players.Add(Host.Guid, Host);
    }

    public Player Join(string username)
    {
        Used();

        Player player = new(username);
        Players.Add(player.Guid, player);
        return player;
    }

    #region Timeout
    private void Used()
    {
        LastUsedTime = DateTime.Now;
    }

    public bool ShouldTimeout()
    {
        return (DateTime.Now - LastUsedTime).TotalSeconds > TimeoutSeconds;
    }
    #endregion
}