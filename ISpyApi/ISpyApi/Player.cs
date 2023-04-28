namespace ISpyApi;

public record Player(string Username)
{
    private const double TimeoutSeconds = 5;

    public Guid Guid { get; init; } = Guid.NewGuid();
    public DateTime LastUsedTimeUtc { get; private set; } = DateTime.UtcNow;

    #region Timeout
    public void Used()
    {
        LastUsedTimeUtc = DateTime.UtcNow;
    }

    public bool ShouldTimeout()
    {
        return (DateTime.UtcNow - LastUsedTimeUtc).TotalSeconds > TimeoutSeconds;
    }
    #endregion
}