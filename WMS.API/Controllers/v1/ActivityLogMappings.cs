using AutoMapper;
using WMS.Models.Entities;
using WMS.Models.Webhooks.v1.Queries.GetWebhookLogs;

namespace WMS.Api.Controllers.v1;

public class ActivityLogMappings : Profile
{
    public ActivityLogMappings()
    {
        CreateMap<ActivityLog, WebhookAcitvityLogModel>().ReverseMap();
    }
}