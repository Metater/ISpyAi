﻿public class FibbageManager : Manager
{
    public override void Connected(ConnectedState connectedState)
    {
        print("Connected fibbage");
    }

    public override void Disconnected(DisconnectedState disconnectedState)
    {
        print("Disconnected fibbage");
    }

    public override bool SchemaReceived(object schema)
    {
        return false;
    }
}