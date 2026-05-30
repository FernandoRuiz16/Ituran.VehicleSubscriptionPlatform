using Ituran.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ituran.Api.Controllers;

[ApiController]
[Route("api/analytics")]
public class AnalyticsController : ControllerBase
{
    private readonly IWebhookRepository _webhookRepository;

    public AnalyticsController(IWebhookRepository webhookRepository)
    {
        _webhookRepository = webhookRepository;
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary()
    {
        var result = await _webhookRepository.GetAnalyticsSummaryAsync();
        return Ok(result);
    }
}