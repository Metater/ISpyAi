using System.Collections.Generic;
using UnityEngine;

public class FibbageManager : Manager
{
    private const int OptionsCount = 3;

    public RectTransform idleTransform;
    public RectTransform selectionTransform;
    public List<FibbageSelections> selections;
    private FibbageState state = null;
    private FibbagePeriodicUpdate periodicUpdate = null;

    private void Awake()
    {
        selections.ForEach(o => o.Hide());

        gameObject.SetActive(false);
    }

    private void Update()
    {
        switch (periodicUpdate.phase)
        {
            case FibbagePhase.Idle:
                idleTransform.gameObject.SetActive(true);
                selectionTransform.gameObject.SetActive(false);

                if (state.lastOptionsUpdate == null)
                {
                    selections.ForEach(o => o.Hide());
                }
                break;
            case FibbagePhase.Selection:
                idleTransform.gameObject.SetActive(false);
                selectionTransform.gameObject.SetActive(true);

                selections.ForEach(o => o.Show());
                state.lastOptionsUpdate = null;
                break;
        }
    }

    public override void Connected(ConnectedState connectedState)
    {
        state = new FibbageState();
        periodicUpdate = new FibbagePeriodicUpdate
        {
            message = "",
            time = 0,
            phase = FibbagePhase.Idle
        };

        gameObject.SetActive(true);
    }

    public override void Disconnected(DisconnectedState disconnectedState)
    {
        selections.ForEach(o => o.Hide());
        state = null;
        periodicUpdate = null;

        gameObject.SetActive(false);
    }

    public override bool HandleSchema(object schema)
    {
        if (schema is FibbagePeriodicUpdate periodicUpdate)
        {
            this.periodicUpdate = periodicUpdate;
        }
        else if (schema is FibbageOptionsUpdate optionsUpdate)
        {
            selections.ForEach(o => o.Hide());
            state.lastOptionsUpdate = optionsUpdate;

            for (int i = 0; i < OptionsCount; i++)
            {
                StartCoroutine(netManager.ApplyTextureFromUri(optionsUpdate.uris[i], selections[i].image));
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
        SendSchema(new FibbageSelectionUpdate
        {
            index = i
        });
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(Screen.width - 300, 0, 300, 9999));

        if (periodicUpdate != null)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label($"Message: {periodicUpdate.message}");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label($"Time: {periodicUpdate.time}");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label($"Phase: {periodicUpdate.phase}");
            GUILayout.EndHorizontal();
        }

        GUILayout.EndArea();
    }
}