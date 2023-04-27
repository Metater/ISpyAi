using System.Collections.Generic;
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
            case nameof(HostResponse): return FromJson<HostResponse>(json, out schema);
            case nameof(JoinResponse): return FromJson<JoinResponse>(json, out schema);
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

    public static string ToJson(object schema)
    {
        string name = schema!.ToString()!.Split('.').Last();
        return name + "\n" + JsonUtility.ToJson(schema);
    }
}

#nullable disable

[Serializable]
public class HostResponse
{
    public string hostname;
    public SerializableGuid guid;
    public ulong code;
}

[Serializable]
public class JoinResponse
{
    public string username;
    public SerializableGuid guid;
}

[Serializable]
public class PlayersResponse
{
    public List<string> players;
}

#nullable enable