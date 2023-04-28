#if !UNITY_64
using ISpyApi.Utilities;

namespace ISpyApi;

#nullable disable
#else
using System;
using System.Collections.Generic;
using UnityEngine;
#endif

[Serializable]
public class HostResponse
{
    public SerializableGuid guid;
    public string hostname;
    public ulong code;
}

[Serializable]
public class JoinResponse
{
    public SerializableGuid guid;
    public string username;
}

[Serializable]
public class PeriodicUpdate
{
    public long serverFileTimeUtc;
    public List<string> players;
}

[Serializable]
public class TestImageRequest
{

}

[Serializable]
public class TestImageResponse
{
    public string uri;
}

public static class Schemas
{
    public static bool FromJson(string name, string json, out object schema)
    {
        switch (name)
        {
            case nameof(HostResponse): return FromJson<HostResponse>(json, out schema);
            case nameof(JoinResponse): return FromJson<JoinResponse>(json, out schema);
            case nameof(PeriodicUpdate): return FromJson<PeriodicUpdate>(json, out schema);
            case nameof(TestImageRequest): return FromJson<TestImageRequest>(json, out schema);
            case nameof(TestImageResponse): return FromJson<TestImageResponse>(json, out schema);
            default:
                Console.WriteLine($"Unknown schema name: {name}");
                schema = default;
                return false;
        }
    }

    private static bool FromJson<T>(string json, out object schema)
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
        string name = schema.GetType().Name;
        return name + "\n" + JsonUtility.ToJson(schema);
    }
}