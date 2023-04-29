using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FibbageManager : Manager
{
    public List<RawImage> optionRawImages;
    private FibbageState state = null;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public override void Connected(ConnectedState connectedState)
    {
        state = new FibbageState();

        gameObject.SetActive(true);
    }

    public override void Disconnected(DisconnectedState disconnectedState)
    {
        state = null;

        gameObject.SetActive(false);
    }

    public override bool HandleSchema(object schema)
    {
        if (schema is FibbagePeriodicUpdate periodicUpdate)
        {
            state.ApplyPeriodicUpdate(periodicUpdate);
        }
        else if (schema is FibbageOptionsUpdate optionsUpdate)
        {
            for (int i = 0; i < 3; i++)
            {
                StartCoroutine(netManager.ApplyTextureFromUri(optionsUpdate.uris[i], optionRawImages[i]));
            }
        }
        else
        {
            return false;
        }

        return true;
    }

    public void OptionButton(int i)
    {
        // TODO Do something cool
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(Screen.width - 300, 0, 300, 9999));

        if (state != null)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label($"Message: {state.message}");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label($"Time: {state.time}");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label($"Step: {state.step}");
            GUILayout.EndHorizontal();
        }

        GUILayout.EndArea();
    }
}