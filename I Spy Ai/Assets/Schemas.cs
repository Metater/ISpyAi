using System.Linq;
using System;
using UnityEngine;

#nullable enable

public static class Schemas
{
    public static bool FromJson(string name, string json, out object? schema)
    {
        switch (name)
        {
            case nameof(HostRequest):
                return FromJson<HostRequest>(json, out schema);
            default:
                Console.WriteLine($"Unknown schema name: {name}");
                schema = default;
                return false;
        }
    }

    private static bool FromJson<T>(string json, out object? schema)
    {
        try
        {
            schema = JsonUtility.FromJson<T>(json);
            return schema is not null;
        }
        catch
        {
            schema = default;
            return false;
        }
    }

    public static string ToJson<T>(T schema)
    {
        string name = schema!.ToString()!.Split('.').Last();
        return name + "\n" + JsonUtility.ToJson(schema);
    }
}

#nullable disable

[Serializable]
public class HostRequest
{
    public string hostname;
    public float aiPercentage;
}

#nullable enable