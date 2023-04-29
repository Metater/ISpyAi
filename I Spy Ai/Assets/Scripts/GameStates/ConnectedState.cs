using System;
using System.Collections.Generic;

public class ConnectedState : GameState
{
    public readonly bool isHosting;
    public readonly Guid guid;
    public List<string> players = new();

    public ConnectedState(GameManager manager, string gameType, string username, ulong code, bool isHosting, Guid guid) : base(manager, gameType, username, code)
    {
        this.isHosting = isHosting;
        this.guid = guid;
    }
}