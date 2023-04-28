namespace ISpyApi.Factories;

public class CodeFactory
{
    private readonly Random random;
    private ulong nextCode = 0;

    public CodeFactory(Random random)
    {
        this.random = random;
        nextCode += (ulong)random.Next(0, ushort.MaxValue);
    }

    public ulong GetCode()
    {
        ulong code = nextCode;
        nextCode += (ulong)random.Next(0, ushort.MaxValue);
        return code;
    }
}