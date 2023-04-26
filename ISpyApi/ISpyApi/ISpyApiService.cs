using System.Text.Json;
using System;
using System.Threading.Channels;
using ISpyApi.Utilities;

namespace ISpyApi;

public class ISpyApiService : BackgroundService
{
    private readonly Channel<(object schema, GuidBox? guidBox)> channel;
    private readonly ILogger<ISpyApiService> logger;
    private readonly Games games = new();

    public ISpyApiService(Channel<(object schema, GuidBox? guidBox)> channel, ILogger<ISpyApiService> logger)
    {
        this.channel = channel;
        this.logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach ((object schema, GuidBox? guidBox) in channel.Reader.ReadAllAsync(stoppingToken))
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