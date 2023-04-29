public abstract class GameState
{
    protected readonly GameManager manager;
    public string gameType;
    public string username;
    public ulong code;

    public GameState(GameManager manager, string gameType, string username, ulong code)
    {
        this.manager = manager;
        this.gameType = gameType;
        this.username = username;
        this.code = code;
    }
}