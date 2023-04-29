using System.Text;
using ISpyApi.Interfaces;

namespace ISpyApi.Utilities;

public class StringBuilderCell : ITimeout
{
    private readonly object sbLock = new();
    private readonly StringBuilder sb = new();
    public DateTime LastUsedTimeUtc { get; set; } = DateTime.UtcNow;

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
            (this as ITimeout).ResetTimeout();

            string output = sb.ToString();
            sb.Clear();
            return output;
        }
    }
}
