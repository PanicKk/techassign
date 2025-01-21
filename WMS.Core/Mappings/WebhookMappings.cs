using AutoMapper;
using WMS.Models.Entities;
using WMS.Models.Orders.v1.Commands.CreateOrder;
using WMS.Models.Webhooks.v1.Commands.CreateWebhook;
using WMS.Models.Webhooks.v1.Commands.UpdateWebhook;
using WMS.Models.Webhooks.v1.Queries.GetWebhooks;
using WMS.Models.Webhooks.v1.Shared;

namespace WMS.Core.Mappings;

public class WebhookMappings : Profile
{
    public WebhookMappings()
    {
        CreateMap<CreateWebhookCommand, Webhook>().ReverseMap();
        
        CreateMap<UpdateWebhookCommand, Webhook>().ReverseMap();
        
        CreateMap<Webhook, WebhookModel>().ReverseMap();
        
        // CreateMap<Webhook, WebhookListModel>().ReverseMap();
    }
}