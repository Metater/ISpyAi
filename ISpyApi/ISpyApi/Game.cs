﻿using ISpyApi.Factories;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ISpyApi;

public record Game(Random Random, ImageFactory ImageFactory, CodeFactory CodeFactory, string Hostname)
{
    public Player Host { get; init; } = new(Hostname);
    public ulong Code { get; init; } = CodeFactory.GetNextCode();
    public List<Player> Players { get; init; } = new();
    public DateTime LastAccess { get; private set; } = DateTime.Now;

    public void Init()
    {
        Players.Add(Host);
    }

    public bool HasPlayerWithGuid(Guid guid)
    {
        if (Host.Guid == guid)
        {
            return true;
        }

        return Players.Any(p => p.Guid == guid);
    }

    public Player Join(string username)
    {
        Accessed();

        Player player = new(username);
        Players.Add(player);
        return player;
    }

    private void Accessed()
    {
        LastAccess = DateTime.Now;
    }
}