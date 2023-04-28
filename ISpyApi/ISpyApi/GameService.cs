using System.Text.Json;
using System;
using System.Threading.Channels;
using ISpyApi.Utilities;
using System.Collections.Concurrent;

namespace ISpyApi;

public class GameService : BackgroundService
{
    public const int MaxMemory = 500 * 1024 * 1024;
    public const int MaxMessageSize = 80 * 1024;
    public const int MaxChannelSize = MaxMemory / MaxMessageSize;

    private readonly Channel<(Guid guid, List<object> schemas)> input = Channel.CreateBounded<(Guid guid, List<object> schemas)>(MaxChannelSize);
    private readonly ConcurrentDictionary<Guid, StringBuilderCell> output = new();
    
    private readonly object gamesLock = new();
    private readonly Games games;

    public GameService()
    {
        games = new((Guid guid, object schema) =>
        {
            if (!output.TryGetValue(guid, out var sb))
            {
                sb = new();
                output.TryAdd(guid, sb);
            }

            if (output.TryGetValue(guid, out sb))
            {
                sb.AppendLine(Schemas.ToJson(schema));
            }
        });
    }

    public HostResponse Host(string hostname)
    {
        lock (gamesLock)
        {
            var game = games.Host(hostname);
            return new HostResponse
            {
                guid = game.Host.Guid,
                hostname = game.Host.Username,
                code = game.Code
            };
        }
    }

    public bool Join(ulong code, string username, out JoinResponse? response)
    {
        lock (gamesLock)
        {
            if (games.Join(code, username, out Player? player))
            {
                response = new JoinResponse
                {
                    guid = player!.Guid,
                    username = player.Username
                };
                return true;
            }

            response = default;
            return false;
        }
    }

    public bool Input(Guid guid, List<object> schemas)
    {
        return input.Writer.TryWrite((guid, schemas));
    }

    public string Output(Guid guid)
    {
        StringBuilderCell? sb;
        if (!output.TryGetValue(guid, out _))
        {
            sb = new();
            output.TryAdd(guid, sb);
        }

        lock (gamesLock)
        {
            games.RequestPeriodicOutput(guid);
        }

        if (output.TryGetValue(guid, out sb))
        {
            return sb.ToString();
        }

        return "";
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach ((Guid guid, List<object> schemas) in input.Reader.ReadAllAsync(stoppingToken))
        {
            lock (gamesLock)
            {
                foreach (var schema in schemas)
                {
                    games.SchemaReceived(guid, schema);
                }
            }
        }
    }
}