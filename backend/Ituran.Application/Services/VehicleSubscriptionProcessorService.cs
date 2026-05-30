using Ituran.Application.Interfaces;
using Ituran.Domain.Entities;

namespace Ituran.Application.Services;

public class VehicleSubscriptionProcessorService
{
    private readonly IWebhookRepository _webhookRepository;
    private readonly IProcessingLogRepository _processingLogRepository;

    public VehicleSubscriptionProcessorService(
        IWebhookRepository webhookRepository,
        IProcessingLogRepository processingLogRepository)
    {
        _webhookRepository = webhookRepository;
        _processingLogRepository = processingLogRepository;
    }
    /// <summary>
    /// Processes pending vehicle subscriptions asynchronously.
    /// Simulates ERP contract creation and CRM mobile user creation,
    /// updating the item status and writing processing logs for traceability.
    /// </summary>
    public async Task ProcessPendingAsync()
    {
        var items = await _webhookRepository.GetPendingItemsAsync(10);

        foreach (var item in items)
        {
            try
            {
                await AddLog(item, "Worker", "Started", "Processing item started.");

                await _webhookRepository.MarkAsProcessingErpAsync(item.Id);

                await AddLog(item, "ERP", "Processing", "Sending item to ERP to create contract.");

                await Task.Delay(1500);

                var erpContractId = $"ERP-{item.Id}-{DateTime.UtcNow:yyyyMMddHHmmss}";

                await _webhookRepository.MarkErpCompletedAsync(item.Id, erpContractId);

                await AddLog(item, "ERP", "Completed", $"ERP contract created: {erpContractId}");

                await _webhookRepository.MarkAsProcessingCrmAsync(item.Id);

                await AddLog(item, "CRM", "Processing", "Sending item to CRM to create mobile app user.");

                await Task.Delay(1500);

                var crmUserId = $"CRM-{item.Id}-{DateTime.UtcNow:yyyyMMddHHmmss}";

                await _webhookRepository.MarkCompletedAsync(item.Id, crmUserId);

                await AddLog(item, "CRM", "Completed", $"CRM user created: {crmUserId}");

                await AddLog(item, "Worker", "Completed", "Processing item completed successfully.");
            }
            catch (Exception ex)
            {
                await _webhookRepository.MarkFailedAsync(item.Id, ex.Message);

                await AddLog(item, "Worker", "Failed", "Processing item failed.", ex.ToString());
            }
        }
    }

    private async Task AddLog(
        WebhookItem item,
        string step,
        string status,
        string message,
        string? error = null)
    {
        await _processingLogRepository.CreateAsync(new ProcessingLog
        {
            WebhookItemId = item.Id,
            CorrelationId = item.CorrelationId,
            Step = step,
            Status = status,
            Message = message,
            ErrorDetail = error,
            CreatedAt = DateTime.UtcNow
        });
    }
}