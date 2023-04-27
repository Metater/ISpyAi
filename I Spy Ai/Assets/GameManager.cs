using Mono.Cecil.Cil;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour
{
    public float pollPeriod = 0.5f;

    private Guid guid = Guid.Empty;
    private StringBuilder sb;

    private void Awake()
    {
        sb = new();
    }

    private void Start()
    {
        StartCoroutine(Host("Metater"));

        StartCoroutine(PollLoop());
    }

    private IEnumerator PollLoop()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(pollPeriod);

            StartCoroutine(Poll());
        }
    }

    private IEnumerator Host(string hostname)
    {
        using UnityWebRequest request = UnityWebRequest.Get(GetHostUri(hostname));

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log($"Host error: {request.error}");
        }
        else
        {
            string data = request.downloadHandler.text;
            HandleData(data);
        }
    }

    private IEnumerator Join(ulong code, string username)
    {
        using UnityWebRequest request = UnityWebRequest.Get(GetJoinUri(code, username));

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log($"Join error: {request.error}");
        }
        else
        {
            string data = request.downloadHandler.text;
            HandleData(data);
        }
    }

    private IEnumerator Poll()
    {
        if (guid == Guid.Empty)
        {
            yield break;
        }

        string data = sb.ToString();
        sb.Clear();

        using UnityWebRequest request = UnityWebRequest.Post(GetPollUri(guid), data);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log($"Poll error: {request.error}");
        }
        else
        {
            data = request.downloadHandler.text;
            HandleData(data);
        }
    }

    private void HandleData(string data)
    {
        string[] lines = data.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < lines.Length / 2; i += 2)
        {
            string name = lines[i];
            string json = lines[i + 1];
            if (Schemas.FromJson(name, json, out object schema))
            {
                HandleSchema(schema);
            }
            else
            {
                print(
                    "Unable to handle data:\n" +
                    $"\tname: {name}\n" +
                    $"\tjson: {json}"
               );
            }
        }
    }

    private void HandleSchema(object schema)
    {
        if (schema is HostResponse hostResponse)
        {
            print(hostResponse.hostname);
            print(hostResponse.guid);
            print(hostResponse.code);

        }
        if (schema is JoinResponse joinResponse)
        {
            print(joinResponse.username);
            print(joinResponse.guid);
        }
        else
        {
            Console.WriteLine($"Got unimplemented schema type: {schema}");
        }
    }

    private static string GetHostUri(string hostname) => $"http://localhost:5092/host/{hostname}";
    private static string GetJoinUri(ulong code, string username) => $"http://localhost:5092/join/{code}/{username}";
    private static string GetPollUri(Guid guid) => $"http://localhost:5092/poll/{guid}";
}
