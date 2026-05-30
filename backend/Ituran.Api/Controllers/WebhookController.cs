using Ituran.Application.DTOs;
using Ituran.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ituran.Api.Controllers;

[ApiController]
[Route("api/webhooks")]
public class WebhookController : ControllerBase
{
    private readonly WebhookIngestionService _webhookIngestionService;

    public WebhookController(WebhookIngestionService webhookIngestionService)
    {
        _webhookIngestionService = webhookIngestionService;
    }

    [HttpPost("vehicle-subscriptions")]
    public async Task<IActionResult> ReceiveVehicleSubscriptions(
        [FromBody] List<VehicleSubscriptionWebhookRequest> request)
    {
        var signature = Request.Headers["X-Signature"].FirstOrDefault();

        var result = await _webhookIngestionService.ReceiveAsync(request, signature);

        return Accepted(result);
    }

}