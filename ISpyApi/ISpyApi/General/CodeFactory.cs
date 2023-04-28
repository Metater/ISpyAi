namespace ISpyApi.General;

public class CodeFactory
{
    private readonly Random random;
    private ulong nextCode = 0;

    public CodeFactory(Random random)
    {
        this.random = random;
        nextCode += GetIncrement();
    }

    public ulong GetCode()
    {
        ulong code = nextCode;
        nextCode += GetIncrement();
        return code;
    }

    private ulong GetIncrement()
    {
        return (ulong)random.Next(0, ushort.MaxValue);
    }
}