using System;
using System.Collections.Generic;

public class ConnectedState : GameState
{
    public readonly bool isHosting;
    public readonly Guid guid;
    public long serverFileTimeUtc = DateTime.UtcNow.ToFileTimeUtc();
    public List<string> players = new();

    public ConnectedState(GameManager manager, string username, ulong code, bool isHosting, Guid guid) : base(manager, username, code)
    {
        this.isHosting = isHosting;
        this.guid = guid;
    }
}