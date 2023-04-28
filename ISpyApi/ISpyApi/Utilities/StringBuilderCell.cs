using System.Text;

namespace ISpyApi.Utilities;

public class StringBuilderCell : ITimeout
{
    private readonly object sbLock = new();
    private readonly StringBuilder sb = new();
    private DateTime lastUsedTimeUtc = DateTime.UtcNow;

    public void AppendLine(string value)
    {
        lock (sbLock)
        {
            sb.AppendLine(value);
        }
    }

    public override string ToString()
    {
        lock (sbLock)
        {
            lastUsedTimeUtc = DateTime.UtcNow;

            string output = sb.ToString();
            sb.Clear();
            return output;
        }
    }

    public bool ShouldTimeout(double timeoutSeconds)
    {
        lock (sbLock)
        {
            return (DateTime.UtcNow - LastUsedTimeUtc).TotalSeconds > timeoutSeconds;
        }
    }
}
