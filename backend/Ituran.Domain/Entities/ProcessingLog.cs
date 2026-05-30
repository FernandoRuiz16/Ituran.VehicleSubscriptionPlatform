using System;
using System.Collections.Generic;
using System.Text;

namespace Ituran.Domain.Entities;

public class ProcessingLog
{
    public int Id { get; set; }

    public int WebhookItemId { get; set; }

    public Guid CorrelationId { get; set; }

    public string Step { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public string? Message { get; set; }

    public string? ErrorDetail { get; set; }

    public int? DurationMs { get; set; }

    public DateTime CreatedAt { get; set; }

    public WebhookItem WebhookItem { get; set; } = null!;
}