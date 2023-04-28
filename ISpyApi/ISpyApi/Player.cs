namespace ISpyApi;

public record Player(string Username)
{
    private const double TimeoutSeconds = 5;

    public Guid Guid { get; init; } = Guid.NewGuid();
    public DateTime LastUsedTime { get; private set; } = DateTime.Now;

    #region Timeout
    public void Used()
    {
        LastUsedTime = DateTime.Now;
    }

    public bool ShouldTimeout()
    {
        return (DateTime.Now - LastUsedTime).TotalSeconds > TimeoutSeconds;
    }
    #endregion
}