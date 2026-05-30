namespace Ituran.Application.DTOs;

public class DashboardSummaryDto
{
    public int TotalItems { get; set; }
    public int Completed { get; set; }
    public int Pending { get; set; }
    public int Processing { get; set; }
    public int Retrying { get; set; }
    public int DeadLetter { get; set; }
    public int Failed { get; set; }
    public int TotalBatches { get; set; }
}