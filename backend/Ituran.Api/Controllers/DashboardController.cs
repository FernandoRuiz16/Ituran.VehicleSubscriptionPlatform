using Ituran.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ituran.Api.Controllers;

[ApiController]
[Route("api/dashboard")]
public class DashboardController : ControllerBase
{
    private readonly IWebhookRepository _webhookRepository;

    public DashboardController(IWebhookRepository webhookRepository)
    {
        _webhookRepository = webhookRepository;
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary()
    {
        var result = await _webhookRepository.GetDashboardSummaryAsync();
        return Ok(result);
    }
}