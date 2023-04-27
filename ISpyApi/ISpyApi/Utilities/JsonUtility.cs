namespace ISpyApi.Utilities;

using System.Text.Json;

public static class JsonUtility
{
    private static readonly JsonSerializerOptions options = new() { IncludeFields = true };

    public static string ToJson(object value)
    {
        return JsonSerializer.Serialize(value, options);
    }

    public static T? FromJson<T>(string json)
    {
        return JsonSerializer.Deserialize<T>(json, options);
    }
}