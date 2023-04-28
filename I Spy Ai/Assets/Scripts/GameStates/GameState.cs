public abstract class GameState
{
    protected readonly GameManager manager;
    public string username;
    public ulong code;

    public GameState(GameManager manager, string username, ulong code)
    {
        this.manager = manager;
        this.username = username;
        this.code = code;
    }
}