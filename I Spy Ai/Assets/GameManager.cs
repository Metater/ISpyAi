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
        StartCoroutine(PollLoop());
    }

    private void FixedUpdate()
    {
        sb.AppendLine(Schemas.ToJson(new HostRequest()
        {
            hostname = "localhost",
            aiPercentage = Time.time,
        }));
    }

    private IEnumerator PollLoop()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(pollPeriod);

            StartCoroutine(Poll());
        }
    }

    private IEnumerator Poll()
    {
        if (guid != Guid.Empty)
        {
            sb.Insert(0, guid.ToString() + "\n");
        }

        string data = sb.ToString();
        sb.Clear();

        using UnityWebRequest request = UnityWebRequest.Post("http://localhost:5092/poll", data);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);

            // TODO Disconnect from server or reset logic or something
        }
        else
        {
            data = request.downloadHandler.text;
            string[] lines = data.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            if (lines.Length > 0)
            {
                if (Guid.TryParse(lines[0], out Guid guid) && guid != Guid.Empty)
                {
                    this.guid = guid;
                }
                else
                {
                    // TODO Read data
                }
            }
        }
    }
}
