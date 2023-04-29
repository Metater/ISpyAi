using ISpyApi.Models;

namespace ISpyApi.Fibbage;

public class FibbageGame : Game
{
    public new const string GameType = "Fibbage";
    private const int OptionsCount = 3;
    private const double RoundStartSeconds = 5;
    private const double RoundSelectionPhaseSeconds = 10;
    private const bool HighResImages = false;

    private FibbageImageCategory imageCategory = FibbageImageCategory.Animal;
    private readonly FibbagePeriodicUpdate periodicUpdate = new()
    {
        message = "",
        time = 0,
        phase = FibbagePhase.Idle
    };
    private double roundTime = 0;
    private FibbageGuessesUpdate? lastSendGuessesUpdate = null;

    public FibbageGame(Resources resources, string hostname) : base(resources, GameType, hostname)
    {

    }

    protected override Player? InternalCreatePlayer(bool isHost, string username)
    {
        if (isHost)
        {
            return new FibbagePlayer(this, true, username);
        }

        if (periodicUpdate.phase == FibbagePhase.Idle)
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
            periodicUpdate.phase = FibbagePhase.Idle;
            roundTime = 0;
        }
        else
        {
            if (periodicUpdate.phase == FibbagePhase.Idle)
            {
                double timeUntilRoundStart = RoundStartSeconds - roundTime;
                if (timeUntilRoundStart > 0)
                {
                    // Start round once round start seconds has elasped
                    periodicUpdate.message = $"Round starting in {(int)Math.Round(timeUntilRoundStart)} seconds...";
                }
                else
                {
                    periodicUpdate.phase = FibbagePhase.Selection;

                    foreach ((Guid guid, Player player) in Players)
                    {
                        FibbagePlayer fibbagePlayer = (FibbagePlayer)player;

                        fibbagePlayer.LastSentOptionsUpdate = new FibbageOptionsUpdate
                        {
                            uris = GetOptionUris()
                        };

                        SendSchema(guid, fibbagePlayer.LastSentOptionsUpdate);
                    }
                }
            }
            else if (periodicUpdate.phase == FibbagePhase.Selection)
            {
                double timeUntilGuessingStart = RoundStartSeconds + RoundSelectionPhaseSeconds - roundTime;
                if (timeUntilGuessingStart > 0)
                {
                    periodicUpdate.message = $"Selection ending in {(int)Math.Round(timeUntilGuessingStart)} seconds...";
                }
                else
                {
                    periodicUpdate.phase = FibbagePhase.Guessing;

                    lastSendGuessesUpdate = new FibbageGuessesUpdate
                    {
                        uris = GetGuessUris()
                    };

                    foreach (Guid guid in Players.Keys)
                    {
                        SendSchema(guid, lastSendGuessesUpdate);
                    }
                }
            }
            else if (periodicUpdate.phase == FibbagePhase.Guessing)
            {

            }

            roundTime += deltaTime;
        }

        periodicUpdate.time += deltaTime;
    }

    protected override bool InternalHandleSchema(Guid guid, object schema)
    {
        if (schema is FibbageSelectionUpdate selectionUpdate)
        {
            if (Players.TryGetValue(guid, out var player))
            {
                FibbagePlayer fibbagePlayer = (FibbagePlayer)player;
                fibbagePlayer.SelectionIndex = selectionUpdate.index;
            }
        }
        else
        {
            return false;
        }

        return true;
    }

    protected override void InternalRequestPeriodicOutput(Guid guid)
    {
        SendSchema(guid, periodicUpdate);
    }

    private List<string> GetOptionUris()
    {
        List<string> options = new();
        for (int i = 0; i < OptionsCount; i++)
        {
            options.Add(GetRandomUri(true));
        }
        return options;
    }

    private List<string> GetGuessUris()
    {
        List<string> guesses = new();

        foreach (Player player in Players.Values)
        {
            FibbagePlayer fibbagePlayer = (FibbagePlayer)player;

            // If given an invalid selection, randomize it within bounds
            if (fibbagePlayer.SelectionIndex < 0 || fibbagePlayer.SelectionIndex >= OptionsCount)
            {
                fibbagePlayer.SelectionIndex = resources.Random.Next(0, OptionsCount);
            }

            string guess = fibbagePlayer.LastSentOptionsUpdate!.uris[fibbagePlayer.SelectionIndex];
            guesses.Add(guess);
        }

        guesses.Add(GetRandomUri(false));

        return guesses;
    }

    private string GetRandomUri(bool isAi)
    {
        if (isAi)
        {
            return imageCategory switch
            {
                FibbageImageCategory.Animal => resources.ImageFactory.GetRandomAiAnimalImageUri(HighResImages),
                FibbageImageCategory.Art => resources.ImageFactory.GetRandomAiArtImageUri(HighResImages),
                FibbageImageCategory.Photog => resources.ImageFactory.GetRandomAiPhotogImageUri(HighResImages),
                _ => resources.ImageFactory.GetRandomAiPhotogImageUri(HighResImages)
            };
        }

        return imageCategory switch
        {
            FibbageImageCategory.Animal => resources.ImageFactory.GetRandomRealAnimalImageUri(HighResImages),
            FibbageImageCategory.Art => resources.ImageFactory.GetRandomRealArtImageUri(HighResImages),
            FibbageImageCategory.Photog => resources.ImageFactory.GetRandomRealPhotogImageUri(HighResImages),
            _ => resources.ImageFactory.GetRandomRealPhotogImageUri(HighResImages)
        };
    }
}