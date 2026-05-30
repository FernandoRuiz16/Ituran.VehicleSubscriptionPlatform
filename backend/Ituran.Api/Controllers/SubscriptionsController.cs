using Ituran.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ituran.Api.Controllers;

[ApiController]
[Route("api/subscriptions")]
public class SubscriptionsController : ControllerBase
{
    private readonly IWebhookRepository _webhookRepository;

    public SubscriptionsController(IWebhookRepository webhookRepository)
    {
        _webhookRepository = webhookRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetSubscriptions()
    {
        var result = await _webhookRepository.GetSubscriptionsAsync();
        return Ok(result);
    }

    [HttpGet("{id:int}/logs")]
    public async Task<IActionResult> GetLogs(int id)
    {
        var result = await _webhookRepository.GetLogsByItemIdAsync(id);
        return Ok(result);
    }
 
}