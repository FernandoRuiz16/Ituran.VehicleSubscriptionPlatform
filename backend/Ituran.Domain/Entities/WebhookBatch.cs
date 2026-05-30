using System;
using System.Collections.Generic;
namespace Ituran.Domain.Entities;

public class WebhookBatch
{
    public int Id { get; set; }

    public Guid CorrelationId { get; set; }

    public string Source { get; set; } = string.Empty;

    public DateTime ReceivedAt { get; set; }

    public int TotalItems { get; set; }

    public string Status { get; set; } = string.Empty;

    public string RawJson { get; set; } = string.Empty;

    public string? Signature { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public ICollection<WebhookItem> Items { get; set; } = new List<WebhookItem>();
}