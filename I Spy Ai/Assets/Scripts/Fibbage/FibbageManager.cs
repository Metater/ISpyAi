using UnityEngine;

public class FibbageManager : Manager
{
    private string state = "";

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public override void Connected(ConnectedState connectedState)
    {
        gameObject.SetActive(true);
    }

    public override void Disconnected(DisconnectedState disconnectedState)
    {
        gameObject.SetActive(false);
    }

    public override bool HandleSchema(object schema)
    {
        if (schema is FibbagePeriodicUpdate fibbagePeriodicUpdate)
        {
            state = fibbagePeriodicUpdate.state;
        }
        else
        {
            return false;
        }

        return true;
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(Screen.width - 300, 0, 300, 9999));

        GUILayout.BeginHorizontal();
        GUILayout.Label(state);
        GUILayout.EndHorizontal();

        GUILayout.EndArea();
    }
}