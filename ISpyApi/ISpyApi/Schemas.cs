#region Declarations
using System.Reflection;

#if !UNITY_64
using ISpyApi.Utilities;

namespace ISpyApi;

#nullable disable
#else
using System;
using System.Collections.Generic;
using UnityEngine;
#endif
#endregion

#region General Schemas
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
#endregion

#region Schemas
public static class Schemas
{
    private static Assembly assembly;
    private static readonly Dictionary<string, Type> types = new();

    public static bool FromJson(string name, string json, out object schema)
    {
        assembly ??= Assembly.GetCallingAssembly();

        if (!types.TryGetValue(name, out Type type))
        {
            foreach (var t in assembly.GetTypes())
            {
                if (t.Name == name)
                {
                    type = t;
                    break;
                }
            }

            types.Add(name, type);
        }

        if (type is not null)
        {
            return FromJson(json, type, out schema);
        }

#if UNITY_64
        Debug.Log($"Unknown schema name: {name}");
#else
        Console.WriteLine($"Unknown schema name: {name}");
#endif
        schema = default;
        return false;
    }

    private static bool FromJson(string json, Type type, out object schema)
    {
        try
        {
            schema = JsonUtility.FromJson(json, type);
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
#endregion