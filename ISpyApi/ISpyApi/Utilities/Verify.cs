namespace ISpyApi.Utilities;

using System.Text.RegularExpressions;

public static partial class Verify
{
    [GeneratedRegex("[^a-zA-Z0-9]*$")]
    private static partial Regex UsernameRegex();

    public static bool Username(ref string username)
    {
        username = UsernameRegex().Replace(username, "");

        return username.Length > 0;
    }

    public static bool AiPercentage(float aiPercentage)
    {
        return aiPercentage >= 0 && aiPercentage <= 1;
    }
}
