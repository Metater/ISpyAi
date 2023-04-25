namespace ISpyApi;

public record Player(string Username)
{
    public Guid Guid { get; init; } = Guid.NewGuid();
}