using ISpyApi.Models;

namespace ISpyApi.Fibbage;

public class FibbageGame : Game
{
    public new const string GameType = "Fibbage";

    public FibbageGame(Resources resources, string hostname) : base(resources, GameType, hostname)
    {

    }

    protected override Player CreatePlayer(bool isHost, string username)
    {
        return new FibbagePlayer(this, username);
    }

    protected override void InternalTick(double deltaTime)
    {

    }

    protected override bool InternalHandleSchema(Guid guid, object schema)
    {
        return true;
    }
}