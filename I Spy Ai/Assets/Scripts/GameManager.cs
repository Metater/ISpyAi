using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float netPollPeriod = 0.5f;
    public NetManager netManager;
    public List<Manager> managers;
    private GameState lastState = null;
    public GameState State { get; private set; }

    private void Awake()
    {
        State = new DisconnectedState(this, "Unnamed", 0);

        netManager.OnSchemaReceived += OnSchemaReceived;
        netManager.OnDisconnected += OnDisconnected;
    }

    private void Start()
    {
        StartCoroutine(PollLoop());
    }

    private void Update()
    {
        if (lastState != State)
        {
            if (lastState is not ConnectedState && State is ConnectedState connectedState)
            {
                managers.ForEach(m => m.Connected(connectedState));
            }
            else if (lastState is not DisconnectedState && State is DisconnectedState disconnectedState)
            {
                managers.ForEach(m => m.Disconnected(disconnectedState));
            }

            lastState = State;
        }
    }

    private void OnSchemaReceived(object schema)
    {
        if (schema is HostResponse hostResponse)
        {
            if (State is DisconnectedState)
            {
                netManager.ClearQueuedData();

                State = new ConnectedState(this, hostResponse.hostname, hostResponse.code, true, hostResponse.guid);
            }
        }
        else if (schema is JoinResponse joinResponse)
        {
            if (State is DisconnectedState disconnectedState)
            {
                netManager.ClearQueuedData();

                State = new ConnectedState(this, joinResponse.username, disconnectedState.code, true, joinResponse.guid);
            }
        }
        else if (schema is PeriodicUpdate periodicUpdate)
        {
            if (State is ConnectedState connectedState)
            {
                connectedState.serverFileTimeUtc = periodicUpdate.serverFileTimeUtc;
                connectedState.players = periodicUpdate.players;
            }
        }
        else
        {
            bool handled = false;
            foreach (var manager in managers)
            {
                if (manager.SchemaReceived(schema))
                {
                    handled = true;
                    break;
                }
            }

            if (!handled)
            {
                print($"Got unimplemented schema with name: {schema}");
            }
        }
    }

    private void OnDisconnected()
    {
        State = new DisconnectedState(this, State.username, State.code);
    }

    private IEnumerator PollLoop()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(netPollPeriod);

            if (State is ConnectedState connectedState)
            {
                StartCoroutine(netManager.Poll(connectedState.guid));
            }
        }
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(0, 0, 300, 9999));

        GUILayout.BeginHorizontal();
        GUILayout.Label($"State: {State.GetType().Name}");
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (State is DisconnectedState)
        {
            GUILayout.Label("Username: ");
            State.username = GUILayout.TextField(State.username);
            if (State.username.Length > 24)
            {
                State.username = State.username[..24];
            }
        }
        else
        {
            GUILayout.Label($"Username: {State.username}");
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (State is DisconnectedState)
        {
            GUILayout.Label("Code: ");
            string codeText = GUILayout.TextField(State.code.ToString());
            if (codeText == "")
            {
                codeText = "0";
            }
            if (ulong.TryParse(codeText, out ulong code))
            {
                State.code = code;
            }
        }
        else
        {
            GUILayout.Label($"Code: {State.code}");
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Copy Code"))
        {
            GUIUtility.systemCopyBuffer = State.code.ToString();
        }
        if (State is DisconnectedState)
        {
            if (GUILayout.Button("Paste Code"))
            {
                if (ulong.TryParse(GUIUtility.systemCopyBuffer, out ulong code))
                {
                    State.code = code;
                }
            }
        }
        GUILayout.EndHorizontal();

        if (State is DisconnectedState)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Host"))
            {
                StartCoroutine(netManager.Host(State.username));
            }
            if (GUILayout.Button("Join"))
            {
                StartCoroutine(netManager.Join(State.code, State.username));
            }
            GUILayout.EndHorizontal();
        }
        else
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Disconnect"))
            {
                State = new DisconnectedState(this, State.username, State.code);
            }
            GUILayout.EndHorizontal();
        }

        if (State is ConnectedState connectedState)
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
