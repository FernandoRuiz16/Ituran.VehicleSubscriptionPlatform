using System;
using System.Collections.Generic;
using System.Text;

namespace Ituran.Domain.Entities;

public class WebhookItem
{
    public int Id { get; set; }

    public int BatchId { get; set; }

    public Guid CorrelationId { get; set; }

    public string Vin { get; set; } = string.Empty;

    public string? Placa { get; set; }

    public string? Dispositivo { get; set; }

    public DateTime? FechaEnvio { get; set; }

    public DateTime? FechaInicio { get; set; }

    public DateTime? FechaFinal { get; set; }

    public int? EventoId { get; set; }

    public string? Evento { get; set; }

    public string? Orden { get; set; }

    public string? Nombre { get; set; }

    public string? Apellidos { get; set; }

    public string? Correo { get; set; }

    public string Status { get; set; } = string.Empty;

    public int RetryCount { get; set; }

    public string IdempotencyKey { get; set; } = string.Empty;

    public string? ErpContractId { get; set; }

    public string? CrmUserId { get; set; }

    public string? LastError { get; set; }

    public DateTime? LockedAt { get; set; }

    public DateTime? ProcessedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public WebhookBatch Batch { get; set; } = null!;
}
