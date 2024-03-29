﻿using ISpyApi.Interfaces;

namespace ISpyApi.Models;

public abstract record Player(Game Game, bool IsHost, string Username) : ITimeout
{
    public Guid Guid { get; init; } = Guid.NewGuid();
    public DateTime LastUsedTimeUtc { get; set; } = DateTime.UtcNow;
}