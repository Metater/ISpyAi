namespace ISpyApi;

using System.Text.Json;
using System.Text.Json.Serialization;

public static class Json
{
    private static readonly JsonSerializerOptions options = new() { IncludeFields = true };

    public static string Serialize<T>(T data)
    {
        return JsonSerializer.Serialize(data, options);
    }

    public static T? Deserialize<T>(string json)
    {
        return JsonSerializer.Deserialize(json, options);
    }
}