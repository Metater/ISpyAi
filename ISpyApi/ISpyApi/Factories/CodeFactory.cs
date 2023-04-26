namespace ISpyApi.Factories;

public class CodeFactory
{
    private ulong nextCode = 0;

    public ulong GetNextCode() => nextCode++;
}