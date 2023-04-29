public class FibbageState
{
    public string message = "";
    public double time = 0;
    public FibbageStep step = FibbageStep.Idle;

    public FibbageState()
    {

    }

    public void ApplyPeriodicUpdate(FibbagePeriodicUpdate periodicUpdate)
    {
        message = periodicUpdate.message;
        time = periodicUpdate.time;
        step = periodicUpdate.step;
    }
}