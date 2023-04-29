using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetManager : MonoBehaviour
{
    public int normalTimeoutSeconds = 2;
    public int textureTimeoutSeconds = 4;
    private StringBuilder sb;
    public event Action<object> OnSchemaReceived;
    public event Action OnDisconnected;

    private void Awake()
    {
        sb = new();

        OnDisconnected += ClearQueuedData;
    }

    public void ClearQueuedData()
    {
        sb.Clear();
    }

    public void SendSchema(object schema)
    {
        sb.AppendLine(Schemas.ToJson(schema));
    }

    public IEnumerator Host(string gameType, string hostname)
    {
        using UnityWebRequest request = UnityWebRequest.Get(GetHostUri(gameType, hostname));

        request.timeout = normalTimeoutSeconds;

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            print($"Host error: {request.error}");
            OnDisconnected?.Invoke();
        }
        else
        {
            HandleData(request.downloadHandler.text);
        }
    }

    public IEnumerator Join(ulong code, string username)
    {
        using UnityWebRequest request = UnityWebRequest.Get(GetJoinUri(code, username));

        request.timeout = normalTimeoutSeconds;

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            print($"Join error: {request.error}");
            OnDisconnected?.Invoke();
        }
        else
        {
            HandleData(request.downloadHandler.text);
        }
    }

    public IEnumerator Poll(Guid guid)
    {
        string data = sb.ToString();
        sb.Clear();

        using UnityWebRequest request = UnityWebRequest.Post(GetPollUri(guid), data);

        request.timeout = normalTimeoutSeconds;

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            print($"Poll error: {request.error}");
            OnDisconnected?.Invoke();
        }
        else
        {
            HandleData(request.downloadHandler.text);
        }
    }

    public IEnumerator ApplyTextureFromUri(string uri, RawImage image)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(uri);

        request.timeout = textureTimeoutSeconds;

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            print($"Apply texture from uri error: {request.error}");
        }
        else
        {
            image.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
        }
    }

    private void HandleData(string data)
    {
        if (data.Trim() == "error")
        {
            print("Got error in data handler");
            return;
        }

        string[] lines = data.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        if (lines.Length > 1)
        {
            for (int i = 0; i <= lines.Length / 2; i += 2)
            {
                string name = lines[i];
                string json = lines[i + 1];

                if (Schemas.FromJson(name, json, out object schema))
                {
                    OnSchemaReceived(schema);
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
    }

    private static string GetHostUri(string gameType, string hostname) => $"http://75.0.193.55:44464/host/{gameType}/{hostname}";
    private static string GetJoinUri(ulong code, string username) => $"http://75.0.193.55:44464/join/{code}/{username}";
    private static string GetPollUri(Guid guid) => $"http://75.0.193.55:44464/poll/{guid}";
}
