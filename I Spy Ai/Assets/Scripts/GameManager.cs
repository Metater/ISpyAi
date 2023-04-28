using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float netPollPeriod = 0.5f;
    public NetManager netManager;
    public RawImage testImage;
    private GameState state;

    private void Awake()
    {
        state = new DisconnectedState(this, "Unnamed", 0);

        netManager.OnSchemaReceived += OnSchemaReceived;
        netManager.OnDisconnected += OnDisconnected;
    }

    private void Start()
    {
        StartCoroutine(PollLoop());
    }

    private void OnSchemaReceived(object schema)
    {
        if (schema is HostResponse hostResponse)
        {
            if (state is DisconnectedState)
            {
                netManager.ClearQueuedData();

                state = new ConnectedState(this, hostResponse.hostname, hostResponse.code, true, hostResponse.guid);
            }

            netManager.SendSchema(new TestImageRequest());
        }
        else if (schema is JoinResponse joinResponse)
        {
            if (state is DisconnectedState disconnectedState)
            {
                netManager.ClearQueuedData();

                state = new ConnectedState(this, joinResponse.username, disconnectedState.code, true, joinResponse.guid);
            }

            netManager.SendSchema(new TestImageRequest());
        }
        else if (schema is PeriodicUpdate periodicUpdate)
        {
            if (state is ConnectedState connectedState)
            {
                connectedState.serverFileTimeUtc = periodicUpdate.serverFileTimeUtc;
                connectedState.players = periodicUpdate.players;
            }
        }
        else if (schema is TestImageResponse testImageResponse)
        {
            StartCoroutine(netManager.ApplyTextureFromUri(testImageResponse.uri, testImage));
        }
        else
        {
            print($"Got unimplemented schema with name: {schema}");
        }
    }

    private void OnDisconnected()
    {
        state = new DisconnectedState(this, state.username, state.code);
    }

    private IEnumerator PollLoop()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(netPollPeriod);

            if (state is ConnectedState connectedState)
            {
                StartCoroutine(netManager.Poll(connectedState.guid));
            }
        }
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(0, 0, 300, 9999));

        GUILayout.BeginHorizontal();
        GUILayout.Label($"State: {state.GetType().Name}");
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (state is DisconnectedState)
        {
            GUILayout.Label("Username: ");
            state.username = GUILayout.TextField(state.username);
        }
        else
        {
            GUILayout.Label($"Username: {state.username}");
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (state is DisconnectedState)
        {
            GUILayout.Label("Code: ");
            string codeText = GUILayout.TextField(state.code.ToString());
            if (codeText == "")
            {
                codeText = "0";
            }
            if (ulong.TryParse(codeText, out ulong code))
            {
                state.code = code;
            }
        }
        else
        {
            GUILayout.Label($"Code: {state.code}");
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Copy Code"))
        {
            GUIUtility.systemCopyBuffer = state.code.ToString();
        }
        if (state is DisconnectedState)
        {
            if (GUILayout.Button("Paste Code"))
            {
                if (ulong.TryParse(GUIUtility.systemCopyBuffer, out ulong code))
                {
                    state.code = code;
                }
            }
        }
        GUILayout.EndHorizontal();

        if (state is DisconnectedState)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Host"))
            {
                StartCoroutine(netManager.Host(state.username));
            }
            if (GUILayout.Button("Join"))
            {
                StartCoroutine(netManager.Join(state.code, state.username));
            }
            GUILayout.EndHorizontal();
        }
        else
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Disconnect"))
            {
                state = new DisconnectedState(this, state.username, state.code);
            }
            GUILayout.EndHorizontal();
        }

        if (state is ConnectedState connectedState)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label($"Server File Time Utc: {connectedState.serverFileTimeUtc}");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Players:");
            GUILayout.EndHorizontal();

            for (int i = 0; i < connectedState.players.Count; i++)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label($"\t{i + 1}. {connectedState.players[i]}");
                GUILayout.EndHorizontal();
            }
        }

        GUILayout.EndArea();
    }
}
