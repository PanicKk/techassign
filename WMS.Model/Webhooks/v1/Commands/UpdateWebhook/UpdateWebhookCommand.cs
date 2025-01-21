using System.Text.Json.Serialization;
using MediatR;
using WMS.Models.Enums;

namespace WMS.Models.Webhooks.v1.Commands.UpdateWebhook;

public class UpdateWebhookCommand : IRequest<UpdateWebhookResponse>
{
    [JsonIgnore] 
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    public string Description { get; set; }
    public string Url { get; set; }
    public EntityType EntityType { get; set; }
    public List<EventType> EventTypes { get; set; }
    public Dictionary<string, string> CustomPayload { get; set; }
    public AuthenticationType AuthType { get; set; }
    public string Token { get; set; }
    public bool IsActive { get; set; }
}