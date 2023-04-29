using ISpyApi.Models;

namespace ISpyApi.Fibbage;

public class FibbageGame : Game
{
    public new const string GameType = "Fibbage";
    private double time = 0;

    public FibbageGame(Resources resources, string hostname) : base(resources, GameType, hostname)
    {

    }

    protected override Player InternalCreatePlayer(bool isHost, string username)
    {
        return new FibbagePlayer(this, username);
    }

    protected override void InternalTick(double deltaTime)
    {
        time += deltaTime;
    }

    protected override bool InternalHandleSchema(Guid guid, object schema)
    {
        return true;
    }

    protected override void InternalRequestPeriodicOutput(Guid guid)
    {
        SendSchema(guid, new FibbagePeriodicUpdate
        {
            state = time % 2 <= 1 ? "Even" : "Odd"
        });
    }
}