using ISpyApi.Interfaces;

namespace ISpyApi.Models;

public abstract class Game : ITickable, ITimeout
{
    private const double PlayerTimeoutSeconds = 5;

    private readonly Resources resources;
    public string GameType { get; init; }
    public Player Host { get; init; }
    public ulong Code { get; init; }
    public Dictionary<Guid, Player> Players { get; init; } = new();
    public DateTime LastUsedTimeUtc { get; set; } = DateTime.UtcNow;

    public Game(Resources resources, string gameType, string hostname)
    {
        this.resources = resources;
        GameType = gameType;
        Host = CreatePlayer(true, hostname);
        Code = resources.CodeFactory.GetCode();

        Players.Add(Host.Guid, Host);
    }

    public Player Join(string username)
    {
        (this as ITimeout).ResetTimeout();

        Player player = CreatePlayer(false, username);
        Players.Add(player.Guid, player);
        return player;
    }

    public void Tick(double deltaTime)
    {
        List<Guid> toRemove = new();
        foreach (var player in Players.Values)
        {
            if ((player as ITimeout).ShouldTimeout(PlayerTimeoutSeconds))
            {
                toRemove.Add(player.Guid);
            }
        }
        toRemove.ForEach(g => Players.Remove(g));

        InternalTick(deltaTime);
    }

    public bool HandleSchema(Guid guid, object schema)
    {
        (this as ITimeout).ResetTimeout();

        return InternalHandleSchema(guid, schema);
    }

    protected void SendSchema(Guid guid, object schema)
    {
        resources.SendSchema(guid, schema);
    }

    protected abstract Player CreatePlayer(bool isHost, string username);
    protected abstract void InternalTick(double deltaTime);
    protected abstract bool InternalHandleSchema(Guid guid, object schema);
}