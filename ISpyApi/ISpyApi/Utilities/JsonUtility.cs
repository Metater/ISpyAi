using System.Text.Json;

namespace ISpyApi.Utilities;

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