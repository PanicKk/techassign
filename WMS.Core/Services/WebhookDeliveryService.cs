using System.Net;
using System.Text;
using Microsoft.Extensions.Logging;
using WMS.Core.Repositories;
using WMS.Core.Services.IServices;
using WMS.Core.Utilities;
using WMS.Models.Entities;
using WMS.Models.Enums;

namespace WMS.Core.Services;

public class WebhookDeliveryService : IWebhookDeliveryService
{
    private readonly ILogger<WebhookDeliveryService> _logger;
    private readonly IRepository<Webhook> _webhookRepository;
    private readonly IRepository<ActivityLog> _activityLogRepository;
    private readonly HttpClient _httpClient;

    public WebhookDeliveryService(ILogger<WebhookDeliveryService> logger,
                                  IRepository<Webhook> webhookRepository,
                                  IRepository<ActivityLog> activityLogRepository,
                                  HttpClient httpClient)
    {
        _logger = logger;
        _webhookRepository = webhookRepository;
        _activityLogRepository = activityLogRepository;
        _httpClient = httpClient;
    }


    public async Task HandleEvent(EntityType entityType, EventType eventType, object payload)
    {
        var webhooks = await _webhookRepository.GetAllAsync(
            query => query.Where(webhook => webhook.EntityType == entityType &&
                                            webhook.EventTypes.Contains(eventType)));

        foreach (var webhook in webhooks)
            try
            {
                await DeliverWebhookAsync(webhook, entityType, eventType, payload);
            }
            catch (Exception)
            {
                continue;
            }
    }

    private async Task DeliverWebhookAsync(Webhook webhook, EntityType entityType, EventType eventType, object payload)
    {
        using HttpRequestMessage request = new(HttpMethod.Post, webhook.Url);

        var requestContent = webhook.CustomPayload?.Any() == true
            ? CustomPayloadSerializer.SerializeCustomPayload(payload, webhook.CustomPayload)
            : CustomPayloadSerializer.GetJsonPayload(payload);

        request.Content = new StringContent(
            requestContent,
            Encoding.UTF8,
            "application/json"
        );

        AddHeadersToRequest(webhook, request);
        
        try
        {
            var response = await _httpClient.SendAsync(request);
            await SaveActivityLog(webhook, entityType, eventType, response);
        }
        catch (HttpRequestException ex)
        {
            await SaveActivityLog(webhook, entityType, eventType, new HttpResponseMessage
            {
                StatusCode = ex.StatusCode ?? HttpStatusCode.NotFound,
                ReasonPhrase = ex.Message
            });
            throw;
        }
    }
    
    private static void AddHeadersToRequest(Webhook webhook, HttpRequestMessage request)
    {
        switch (webhook.AuthType)
        {
            case AuthenticationType.Basic:
                request.Headers.Add("Authorization", $"Basic {webhook.Token}");
                break;
            case AuthenticationType.Bearer:
                request.Headers.Add("Authorization", $"Bearer {webhook.Token}");
                break;
        }
    }

    private async Task SaveActivityLog(Webhook webhook, EntityType entityType, EventType eventType, HttpResponseMessage response)
    {
        var activityLog = new ActivityLog
        {
            WebhookId = webhook.Id,
            EntityType = entityType,
            EventType = eventType,
            RequestDateTime = DateTime.UtcNow,
            ResponseStatusCode = Convert.ToInt32(response.StatusCode).ToString(),
            Delivered = response.StatusCode == HttpStatusCode.OK
        };
        
        await _activityLogRepository.InsertAsync(activityLog);
    }
}