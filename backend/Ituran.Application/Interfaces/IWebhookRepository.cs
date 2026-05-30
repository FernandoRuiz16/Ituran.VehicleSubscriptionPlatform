using Ituran.Application.DTOs;
using Ituran.Domain.Entities;

namespace Ituran.Application.Interfaces;

public interface IWebhookRepository
{
    Task<int> CreateBatchAsync(WebhookBatch batch);
    Task CreateItemsAsync(IEnumerable<WebhookItem> items);
    Task<bool> ExistsItemByIdempotencyKeyAsync(string idempotencyKey);

    Task<List<WebhookItem>> GetPendingItemsAsync(int take);
    Task MarkAsProcessingErpAsync(int itemId);
    Task MarkErpCompletedAsync(int itemId, string erpContractId);
    Task MarkAsProcessingCrmAsync(int itemId);
    Task MarkCompletedAsync(int itemId, string crmUserId);
    Task MarkFailedAsync(int itemId, string error);


    Task<IEnumerable<dynamic>> GetSubscriptionsAsync();
    //Task<dynamic?> GetDashboardSummaryAsync();
    Task<DashboardSummaryDto> GetDashboardSummaryAsync();
    Task<IEnumerable<dynamic>> GetLogsByItemIdAsync(int itemId);
    Task<AnalyticsSummaryDto> GetAnalyticsSummaryAsync();
}