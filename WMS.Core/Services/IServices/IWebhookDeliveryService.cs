using WMS.Models.Enums;

namespace WMS.Core.Services.IServices;

public interface IWebhookDeliveryService
{
    Task HandleEvent(EntityType entityType, EventType eventType, object payload);
}