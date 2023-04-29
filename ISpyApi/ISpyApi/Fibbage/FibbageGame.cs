using ISpyApi.Models;

namespace ISpyApi.Fibbage;

public class FibbageGame : Game
{
    public new const string GameType = "Fibbage";
    private const double RoundStartSeconds = 5;
    private const bool HighResImages = false;

    private FibbageImageCategory imageCategory = FibbageImageCategory.Animal;
    private readonly FibbagePeriodicUpdate periodicUpdate = new()
    {
        message = "",
        time = 0,
        step = FibbageStep.Idle
    };
    private double roundStartTime = 0;

    public FibbageGame(Resources resources, string hostname) : base(resources, GameType, hostname)
    {

    }

    protected override Player? InternalCreatePlayer(bool isHost, string username)
    {
        if (isHost)
        {
            return new FibbagePlayer(this, true, username);
        }

        if (periodicUpdate.step == FibbageStep.Idle)
        {
            return new FibbagePlayer(this, false, username);
        }

        return null;
    }

    protected override void InternalTick(double deltaTime)
    {
        if (Players.Count < 2)
        {
            // Stop round: reset step and round start time
            periodicUpdate.message = "Waiting for players...";
            periodicUpdate.step = FibbageStep.Idle;
            roundStartTime = 0;
        }
        else
        {
            // Start round once round start seconds has elasped
            double timeUntilRoundStart = RoundStartSeconds - roundStartTime;
            if (timeUntilRoundStart > 0)
            {
                periodicUpdate.message = $"Round starting in {Math.Round(timeUntilRoundStart)} seconds...";
                periodicUpdate.step = FibbageStep.Idle;

                roundStartTime += deltaTime;
            }
            else if (periodicUpdate.step == FibbageStep.Idle)
            {
                periodicUpdate.message = "Round in progress...";
                periodicUpdate.step = FibbageStep.Selection;

                foreach (Guid guid in Players.Keys)
                {
                    SendSchema(guid, new FibbageOptionsUpdate
                    {
                        uris = GetOptionUris()
                    });
                }
            }
        }

        periodicUpdate.time += deltaTime;
    }

    protected override bool InternalHandleSchema(Guid guid, object schema)
    {
        return true;
    }

    protected override void InternalRequestPeriodicOutput(Guid guid)
    {
        SendSchema(guid, periodicUpdate);
    }

    private List<string> GetOptionUris()
    {
        List<string> options = new();
        for (int i = 0; i < 3; i++)
        {
            string option = imageCategory switch
            {
                FibbageImageCategory.Animal => resources.ImageFactory.GetRandomAiAnimalImageUri(HighResImages),
                FibbageImageCategory.Art => resources.ImageFactory.GetRandomAiArtImageUri(HighResImages),
                FibbageImageCategory.Photog => resources.ImageFactory.GetRandomAiPhotogImageUri(HighResImages),
                _ => resources.ImageFactory.GetRandomAiPhotogImageUri(HighResImages)
            };
            options.Add(option);
        }
        return options;
    }
}