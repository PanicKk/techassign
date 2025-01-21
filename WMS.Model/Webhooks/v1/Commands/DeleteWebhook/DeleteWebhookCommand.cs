using System.Text.Json.Serialization;
using MediatR;

namespace WMS.Models.Webhooks.v1.Commands.DeleteWebhook;

public class DeleteWebhookCommand : IRequest<DeleteWebhookResponse>
{
    [JsonIgnore]
    public Guid Id { get; set; }
}