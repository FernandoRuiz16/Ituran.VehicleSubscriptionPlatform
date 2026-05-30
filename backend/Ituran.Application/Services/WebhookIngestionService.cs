using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Ituran.Application.DTOs;
using Ituran.Application.Interfaces;
using Ituran.Domain.Entities;

namespace Ituran.Application.Services;

public class WebhookIngestionService
{
    private readonly IWebhookRepository _webhookRepository;
    private readonly IProcessingLogRepository _processingLogRepository;

    public WebhookIngestionService(
        IWebhookRepository webhookRepository,
        IProcessingLogRepository processingLogRepository)
    {
        _webhookRepository = webhookRepository;
        _processingLogRepository = processingLogRepository;
    }
    /// <summary>
    /// Persists the webhook batch and its individual items using an Inbox Pattern.
    /// Each item receives an idempotency key to avoid duplicate processing.
    /// </summary>
    public async Task<object> ReceiveAsync(
        List<VehicleSubscriptionWebhookRequest> request,
        string? signature)
    {
        if (request == null || request.Count == 0)
            throw new ArgumentException("Webhook payload is empty.");

        var correlationId = Guid.NewGuid();
        var now = DateTime.UtcNow;
        var rawJson = JsonSerializer.Serialize(request);

        var batch = new WebhookBatch
        {
            CorrelationId = correlationId,
            Source = "ExternalClient",
            ReceivedAt = now,
            TotalItems = request.Count,
            Status = "Received",
            RawJson = rawJson,
            Signature = signature,
            CreatedAt = now,
            UpdatedAt = now
        };

        var batchId = await _webhookRepository.CreateBatchAsync(batch);

        var items = new List<WebhookItem>();
        var duplicated = 0;

        foreach (var x in request)
        {
            var idempotencyKey = BuildIdempotencyKey(x);

            var alreadyExists = await _webhookRepository.ExistsItemByIdempotencyKeyAsync(idempotencyKey);

            if (alreadyExists)
            {
                duplicated++;
                continue;
            }

            items.Add(new WebhookItem
            {
                BatchId = batchId,
                CorrelationId = correlationId,
                Vin = x.Vin,
                Placa = x.Placa,
                Dispositivo = x.Dispositivo,
                FechaEnvio = x.FechaEnvio,
                FechaInicio = x.FechaInicio,
                FechaFinal = x.FechaFinal,
                EventoId = x.EventoId,
                Evento = x.Evento,
                Orden = x.Orden,
                Nombre = x.Nombre,
                Apellidos = x.Apellidos,
                Correo = x.Correo,
                Status = "PendingERP",
                RetryCount = 0,
                IdempotencyKey = idempotencyKey,
                CreatedAt = now,
                UpdatedAt = now
            });
        }

        if (items.Any())
        {
            await _webhookRepository.CreateItemsAsync(items);
        }

        //await _processingLogRepository.CreateAsync(new ProcessingLog
        //{
        //    WebhookItemId = items.First().Id,
        //    CorrelationId = correlationId,
        //    Step = "WebhookReceived",
        //    Status = "Received",
        //    Message = $"Webhook batch received with {request.Count} items.",
        //    CreatedAt = now
        //});
        return new
        {
            message = "Webhook received successfully.",
            correlationId,
            totalItems = request.Count,
            insertedItems = items.Count,
            duplicatedItems = duplicated,
            status = "Accepted"
        };
        //return new
        //{
        //    message = "Webhook received successfully.",
        //    correlationId,
        //    totalItems = request.Count,
        //    status = "Accepted"
        //};
    }
    /// <summary>
    /// Builds a deterministic hash based on business fields.
    /// This prevents creating duplicated ERP contracts or CRM users
    /// when the same webhook is received more than once.
    /// </summary>
    private static string BuildIdempotencyKey(VehicleSubscriptionWebhookRequest item)
    {
        var raw = $"{item.Vin}|{item.Dispositivo}|{item.EventoId}|{item.Orden}|{item.FechaEnvio:O}";
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(raw));
        return Convert.ToHexString(bytes);
    }
}