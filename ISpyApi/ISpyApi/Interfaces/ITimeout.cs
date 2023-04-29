namespace ISpyApi.Interfaces;

public interface ITimeout
{
    public DateTime LastUsedTimeUtc { get; protected set; }

    public void ResetTimeout()
    {
        LastUsedTimeUtc = DateTime.UtcNow;
    }

    public bool ShouldTimeout(double timeoutSeconds)
    {
        return (DateTime.UtcNow - LastUsedTimeUtc).TotalSeconds > timeoutSeconds;
    }
}