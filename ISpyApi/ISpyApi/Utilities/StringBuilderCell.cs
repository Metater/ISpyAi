using System.Text;

namespace ISpyApi.Utilities;

public class StringBuilderCell
{
    private readonly object sbLock = new();
    private readonly StringBuilder sb = new();

    public void AppendLine(string value)
    {
        lock (sbLock)
        {
            sb.AppendLine(value);
        }
    }

    public void Clear()
    {
        lock (sbLock)
        {
            sb.Clear();
        }
    }

    public override string ToString()
    {
        lock (sbLock)
        {
            string output = sb.ToString();
            sb.Clear();
            return output;
        }
    }
}
