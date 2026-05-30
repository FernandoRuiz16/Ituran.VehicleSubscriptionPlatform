using Dapper;
using Ituran.Application.DTOs;
using Ituran.Application.Interfaces;
using Ituran.Domain.Entities;

namespace Ituran.Infrastructure.Repositories;

public class WebhookRepository : IWebhookRepository
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public WebhookRepository(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<int> CreateBatchAsync(WebhookBatch batch)
    {
        const string sql = @"
INSERT INTO WebhookBatches
(
    CorrelationId,
    Source,
    ReceivedAt,
    TotalItems,
    Status,
    RawJson,
    Signature,
    CreatedAt,
    UpdatedAt
)
VALUES
(
    @CorrelationId,
    @Source,
    @ReceivedAt,
    @TotalItems,
    @Status,
    @RawJson,
    @Signature,
    @CreatedAt,
    @UpdatedAt
);

SELECT CAST(SCOPE_IDENTITY() AS INT);";

        using var connection = _connectionFactory.CreateConnection();
        return await connection.ExecuteScalarAsync<int>(sql, batch);
    }

    public async Task CreateItemsAsync(IEnumerable<WebhookItem> items)
    {
        const string sql = @"
INSERT INTO WebhookItems
(
    BatchId,
    CorrelationId,
    Vin,
    Placa,
    Dispositivo,
    FechaEnvio,
    FechaInicio,
    FechaFinal,
    EventoId,
    Evento,
    Orden,
    Nombre,
    Apellidos,
    Correo,
    Status,
    RetryCount,
    IdempotencyKey,
    ErpContractId,
    CrmUserId,
    LastError,
    LockedAt,
    ProcessedAt,
    CreatedAt,
    UpdatedAt
)
VALUES
(
    @BatchId,
    @CorrelationId,
    @Vin,
    @Placa,
    @Dispositivo,
    @FechaEnvio,
    @FechaInicio,
    @FechaFinal,
    @EventoId,
    @Evento,
    @Orden,
    @Nombre,
    @Apellidos,
    @Correo,
    @Status,
    @RetryCount,
    @IdempotencyKey,
    @ErpContractId,
    @CrmUserId,
    @LastError,
    @LockedAt,
    @ProcessedAt,
    @CreatedAt,
    @UpdatedAt
);";

        using var connection = _connectionFactory.CreateConnection();
        await connection.ExecuteAsync(sql, items);
    }

    public async Task<bool> ExistsItemByIdempotencyKeyAsync(string idempotencyKey)
    {
        const string sql = @"
SELECT CASE 
    WHEN EXISTS (
        SELECT 1 
        FROM WebhookItems 
        WHERE IdempotencyKey = @IdempotencyKey
    ) THEN 1 
    ELSE 0 
END;";

        using var connection = _connectionFactory.CreateConnection();

        return await connection.ExecuteScalarAsync<bool>(sql, new
        {
            IdempotencyKey = idempotencyKey
        });
    }
    public async Task<List<WebhookItem>> GetPendingItemsAsync(int take)
    {
        const string sql = @"
SELECT TOP (@Take)
    Id,
    BatchId,
    CorrelationId,
    Vin,
    Placa,
    Dispositivo,
    FechaEnvio,
    FechaInicio,
    FechaFinal,
    EventoId,
    Evento,
    Orden,
    Nombre,
    Apellidos,
    Correo,
    Status,
    RetryCount,
    IdempotencyKey,
    ErpContractId,
    CrmUserId,
    LastError,
    LockedAt,
    ProcessedAt,
    CreatedAt,
    UpdatedAt
FROM WebhookItems
WHERE Status IN ('PendingERP', 'Retrying')
ORDER BY CreatedAt ASC;";

        using var connection = _connectionFactory.CreateConnection();

        var result = await connection.QueryAsync<WebhookItem>(sql, new { Take = take });

        return result.ToList();
    }

    public async Task MarkAsProcessingErpAsync(int itemId)
    {
        const string sql = @"
UPDATE WebhookItems
SET 
    Status = 'ProcessingERP',
    LockedAt = SYSDATETIME(),
    UpdatedAt = SYSDATETIME()
WHERE Id = @ItemId;";

        using var connection = _connectionFactory.CreateConnection();

        await connection.ExecuteAsync(sql, new { ItemId = itemId });
    }

    public async Task MarkErpCompletedAsync(int itemId, string erpContractId)
    {
        const string sql = @"
UPDATE WebhookItems
SET 
    Status = 'ERPCompleted',
    ErpContractId = @ErpContractId,
    UpdatedAt = SYSDATETIME()
WHERE Id = @ItemId;";

        using var connection = _connectionFactory.CreateConnection();

        await connection.ExecuteAsync(sql, new
        {
            ItemId = itemId,
            ErpContractId = erpContractId
        });
    }

    public async Task MarkAsProcessingCrmAsync(int itemId)
    {
        const string sql = @"
UPDATE WebhookItems
SET 
    Status = 'ProcessingCRM',
    UpdatedAt = SYSDATETIME()
WHERE Id = @ItemId;";

        using var connection = _connectionFactory.CreateConnection();

        await connection.ExecuteAsync(sql, new { ItemId = itemId });
    }

    public async Task MarkCompletedAsync(int itemId, string crmUserId)
    {
        const string sql = @"
UPDATE WebhookItems
SET 
    Status = 'Completed',
    CrmUserId = @CrmUserId,
    ProcessedAt = SYSDATETIME(),
    UpdatedAt = SYSDATETIME()
WHERE Id = @ItemId;";

        using var connection = _connectionFactory.CreateConnection();

        await connection.ExecuteAsync(sql, new
        {
            ItemId = itemId,
            CrmUserId = crmUserId
        });
    }

    public async Task MarkFailedAsync(int itemId, string error)
    {
        const string sql = @"
UPDATE WebhookItems
SET 
    RetryCount = RetryCount + 1,
    Status = CASE 
        WHEN RetryCount + 1 >= 5 THEN 'DeadLetter'
        ELSE 'Retrying'
    END,
    LastError = @Error,
    UpdatedAt = SYSDATETIME()
WHERE Id = @ItemId;";

        using var connection = _connectionFactory.CreateConnection();

        await connection.ExecuteAsync(sql, new
        {
            ItemId = itemId,
            Error = error
        });
    }
    public async Task<IEnumerable<dynamic>> GetSubscriptionsAsync()
    {
        const string sql = @"
SELECT 
    Id,
    Vin,
    Placa,
    Dispositivo,
    Evento,
    Orden,
    Nombre,
    Apellidos,
    Correo,
    Status,
    RetryCount,
    ErpContractId,
    CrmUserId,
    LastError,
    CreatedAt,
    ProcessedAt
FROM WebhookItems
ORDER BY CreatedAt DESC;";

        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync(sql);
    }

    public async Task<DashboardSummaryDto> GetDashboardSummaryAsync()
    {
        const string sql = @"
SELECT
    COUNT(1) AS TotalItems,
    ISNULL(SUM(CASE WHEN Status = 'Completed' THEN 1 ELSE 0 END), 0) AS Completed,
    ISNULL(SUM(CASE WHEN Status IN ('PendingERP', 'PendingCRM') THEN 1 ELSE 0 END), 0) AS Pending,
    ISNULL(SUM(CASE WHEN Status IN ('ProcessingERP', 'ProcessingCRM') THEN 1 ELSE 0 END), 0) AS Processing,
    ISNULL(SUM(CASE WHEN Status = 'Retrying' THEN 1 ELSE 0 END), 0) AS Retrying,
    ISNULL(SUM(CASE WHEN Status = 'DeadLetter' THEN 1 ELSE 0 END), 0) AS DeadLetter,
    ISNULL(SUM(CASE WHEN Status = 'Failed' THEN 1 ELSE 0 END), 0) AS Failed,
    COUNT(DISTINCT CorrelationId) AS TotalBatches
FROM WebhookItems;";

        using var connection = _connectionFactory.CreateConnection();

        var result = await connection.QueryFirstAsync<DashboardSummaryDto>(sql);

        return result;
    }

    public async Task<IEnumerable<dynamic>> GetLogsByItemIdAsync(int itemId)
    {
        const string sql = @"
SELECT
    Id,
    WebhookItemId,
    CorrelationId,
    Step,
    Status,
    Message,
    ErrorDetail,
    DurationMs,
    CreatedAt
FROM ProcessingLogs
WHERE WebhookItemId = @ItemId
ORDER BY CreatedAt ASC;";

        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync(sql, new { ItemId = itemId });
    }
    public async Task<AnalyticsSummaryDto> GetAnalyticsSummaryAsync()
    {
        const string sql = @"
SELECT
    (SELECT COUNT(1) FROM WebhookBatches) AS TotalBatches,
    (SELECT COUNT(1) FROM WebhookItems) AS TotalItems,
    (SELECT COUNT(1) FROM WebhookItems WHERE Status = 'Completed') AS CompletedItems,
    (SELECT COUNT(1) FROM WebhookItems WHERE Status IN ('Failed', 'DeadLetter')) AS FailedItems,
    (SELECT COUNT(1) FROM ProcessingLogs) AS LogsCount,
    (SELECT COUNT(1) FROM ProcessingLogs WHERE Step = 'ERP') AS ErpSteps,
    (SELECT COUNT(1) FROM ProcessingLogs WHERE Step = 'CRM') AS CrmSteps;";

        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryFirstAsync<AnalyticsSummaryDto>(sql);
    }
}