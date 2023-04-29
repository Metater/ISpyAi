using ISpyApi.Interfaces;

namespace ISpyApi.Models;

public abstract record Player(Game Game, string Username) : ITimeout
{
    private const double TimeoutSeconds = 5;

    public Guid Guid { get; init; } = Guid.NewGuid();
    public DateTime LastUsedTimeUtc { get; set; } = DateTime.UtcNow;
}