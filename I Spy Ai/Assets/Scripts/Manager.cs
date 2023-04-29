using UnityEngine;

public abstract class Manager : MonoBehaviour
{
    protected GameManager manager;
    protected NetManager netManager;
    public string game;

    public void Init(GameManager manager)
    {
        this.manager = manager;
        netManager = manager.netManager;
    }

    public abstract void Connected(ConnectedState connectedState);
    public abstract void Disconnected(DisconnectedState disconnectedState);
    public abstract bool HandleSchema(object schema);

    protected void SendSchema(object schema)
    {
        netManager.SendSchema(schema);
    }
}