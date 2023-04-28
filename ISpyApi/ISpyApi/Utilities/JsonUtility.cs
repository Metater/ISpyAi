namespace ISpyApi.Utilities;

using System.Text.Json;

public static class JsonUtility
{
    private static readonly JsonSerializerOptions options = new() { IncludeFields = true };

    public static string ToJson(object value)
    {
        return JsonSerializer.Serialize(value, options);
    }

    public static object? FromJson(string json, Type type)
    {
        return JsonSerializer.Deserialize(json, type, options);
    }
}