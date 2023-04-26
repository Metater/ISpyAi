namespace ISpyApi.Utilities;

public class GuidCell
{
    private readonly object guidLock = new();
    private Guid guid = Guid.Empty;

    public Guid Get()
    {
        lock (guidLock)
        {
            return guid;
        }
    }

    public void Set(Guid guid)
    {
        lock (guidLock)
        {
            this.guid = guid;
        }
    }
}