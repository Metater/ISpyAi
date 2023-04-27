using ISpyApi.Factories;

namespace ISpyApi;

public class Game()
{
    private const double TimeoutSeconds = 10;

    private readonly Resources resources;
    public Player Host { get; init; }
    public ulong Code { get; init; }
    public Dictionary<Guid, Player> Players { get; init; } = new();
    public DateTime LastAccessedTime { get; private set; } = DateTime.Now;

    public Game(Resources resources, string hostname)
    {
        this.resources = resources;
        Host = new(hostname);
        Code = resources.CodeFactory.GetNextCode();

        Players.Add(Host);
    }

    public Player Join(string username)
    {
        Accessed();

        Player player = new(username);
        Players.Add(player);
        return player;
    }

    #region Timeout
    private void Accessed()
    {
        LastAccessedTime = DateTime.Now;
    }

    public bool ShouldTimeout()
    {
        return (DateTime.Now - LastAccessedTime).TotalSeconds > TimeoutSeconds;
    }
    #endregion
}