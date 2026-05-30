using Ituran.Application.Services;

namespace Ituran.Worker;

public class Worker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<Worker> _logger;

    public Worker(
        IServiceScopeFactory scopeFactory,
        ILogger<Worker> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }
    /// <summary>
    /// Background loop that periodically processes pending vehicle subscription records.
    /// This prevents the webhook endpoint from blocking during long ERP/CRM operations.
    /// </summary>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Vehicle Subscription Worker started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();

                var processor = scope.ServiceProvider
                    .GetRequiredService<VehicleSubscriptionProcessorService>();

                await processor.ProcessPendingAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing pending vehicle subscriptions.");
            }

            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }
}