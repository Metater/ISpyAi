using UnityEngine;

public abstract class Manager : MonoBehaviour
{
    protected GameManager manager;

    public void Init(GameManager manager)
    {
        this.manager = manager;
    }

    public abstract void Connected(ConnectedState connectedState);
    public abstract void Disconnected(DisconnectedState disconnectedState);
    public abstract bool SchemaReceived(object schema);
}