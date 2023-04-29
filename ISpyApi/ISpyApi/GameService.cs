using System.Text.Json;
using System;
using System.Threading.Channels;
using System.Collections.Concurrent;
using ISpyApi.Utilities;

namespace ISpyApi;

public class GameService : BackgroundService, ITickable
{
    public const int MaxMemory = 500 * 1024 * 1024;
    public const int MaxMessageSize = 80 * 1024;
    public const int MaxChannelSize = MaxMemory / MaxMessageSize;
    public const double OutputTimeoutSeconds = 5;

    private readonly Channel<(Guid guid, List<object> schemas)> input = Channel.CreateBounded<(Guid guid, List<object> schemas)>(MaxChannelSize);
    private readonly ConcurrentDictionary<Guid, StringBuilderCell> output = new();
    
    private readonly object gamesLock = new();
    private readonly Games games;

    public GameService()
    {
        // Way for games to send schemas back to clients
        games = new((Guid guid, object schema) =>
        {
            if (!output.TryGetValue(guid, out var sb))
            {
                sb = new();
                output.TryAdd(guid, sb);
            }

            // Avoid race condition, stored and this sb instance could differ
            // sendSchema and RequestSchemas both access from different threads
            if (output.TryGetValue(guid, out sb))
            {
                sb.AppendLine(Schemas.ToJson(schema));
            }
        });
    }

    // Host game and generate response
    public bool Host(string gameType, string hostname, out HostResponse? response)
    {
        response = default;

        lock (gamesLock)
        {
            if (games.Host(gameType, hostname, out var game))
            {
                response = new HostResponse
                {
                    guid = game!.Host.Guid,
                    gameType = game.GameType,
                    hostname = game.Host.Username,
                    code = game.Code
                };
            }
        }

        return response is not null;
    }

    // Join game and generate response
    public bool Join(ulong code, string username, out JoinResponse? response)
    {
        response = default;

        lock (gamesLock)
        {
            if (games.Join(code, username, out var player))
            {
                response = new JoinResponse
                {
                    guid = player!.Guid,
                    username = player.Username
                };
            }
        }

        return response is not null;
    }

    // Queue schemas to be handled for a client
    public bool SupplySchemas(Guid guid, List<object> schemas)
    {
        return input.Writer.TryWrite((guid, schemas));
    }

    // Get queued schemas for a client
    public string RequestSchemas(Guid guid)
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

        // Avoid race condition, stored and this sb instance could differ
        // sendSchema and RequestSchemas both access from different threads
        if (output.TryGetValue(guid, out sb))
        {
            return sb.ToString();
        }

        return "";
    }

    // Pass tick through
    public void Tick(double deltaTime)
    {
        lock (gamesLock)
        {
            // Remove old string builders after timeout
            foreach ((Guid guid, StringBuilderCell sb) in output)
            {
                if (sb.ShouldTimeout(OutputTimeoutSeconds))
                {
                    output.TryRemove(guid, out _);
                }
            }

            games.Tick(deltaTime);
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Handle schemas as they are received
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