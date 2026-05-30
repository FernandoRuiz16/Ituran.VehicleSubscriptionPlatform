using Dapper;
using Ituran.Application.Interfaces;
using Ituran.Domain.Entities;

namespace Ituran.Infrastructure.Repositories;

public class ProcessingLogRepository : IProcessingLogRepository
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public ProcessingLogRepository(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task CreateAsync(ProcessingLog log)
    {
        const string sql = @"
INSERT INTO ProcessingLogs
(
    WebhookItemId,
    CorrelationId,
    Step,
    Status,
    Message,
    ErrorDetail,
    DurationMs,
    CreatedAt
)
VALUES
(
    @WebhookItemId,
    @CorrelationId,
    @Step,
    @Status,
    @Message,
    @ErrorDetail,
    @DurationMs,
    @CreatedAt
);";

        using var connection = _connectionFactory.CreateConnection();
        await connection.ExecuteAsync(sql, log);
    }
}