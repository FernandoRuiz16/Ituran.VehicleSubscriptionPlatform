namespace Ituran.Application.DTOs;

public class AnalyticsSummaryDto
{
    public int TotalBatches { get; set; }
    public int TotalItems { get; set; }
    public int CompletedItems { get; set; }
    public int FailedItems { get; set; }
    public int LogsCount { get; set; }
    public int ErpSteps { get; set; }
    public int CrmSteps { get; set; }
}