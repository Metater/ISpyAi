using System.Text.Json;
using System;
using System.Threading.Channels;
using ISpyApi.Utilities;
using System.Collections.Concurrent;

namespace ISpyApi;

public class ISpyApiService : BackgroundService
{
    private readonly ConcurrentDictionary<Guid, ConcurrentQueue<string>> queuedSchemas = new();
    private readonly Channel<(object schema, GuidCell? guidCell)> channel;
    private readonly ILogger<ISpyApiService> logger;
    private readonly Games games = new();

    public ISpyApiService(Channel<(object schema, GuidCell? guidCell)> channel, ILogger<ISpyApiService> logger)
    {
        this.channel = channel;
        this.logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach ((object schema, GuidCell? guidCell) in channel.Reader.ReadAllAsync(stoppingToken))
        {
            try
            {
                if (schema is HostRequest hostRequest)
                {
                    Console.WriteLine(hostRequest.hostname);
                    Console.WriteLine(hostRequest.aiPercentage);
                }
                else
                {
                    logger.LogInformation("Got unimplemented schema type in service");
                }
            }
            catch (Exception e)
            {
                logger.LogError("Error in service {}", e.Message);
            }
        }
    }
}