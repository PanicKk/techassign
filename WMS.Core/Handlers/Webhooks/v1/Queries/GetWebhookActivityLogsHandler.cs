using System.Net;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using WMS.Core.Exceptions;
using WMS.Core.Extensions;
using WMS.Core.Repositories;
using WMS.Models.Common.Pagination;
using WMS.Models.Entities;
using WMS.Models.Enums;
using WMS.Models.Webhooks.v1.Queries.GetWebhookLogs;

namespace WMS.Core.Handlers.Webhooks.v1.Queries;

public class GetWebhookActivityLogsHandler : IRequestHandler<GetWebhookLogsQuery, IPagedList<WebhookAcitvityLogModel>>
{
    private readonly ILogger<GetWebhookActivityLogsHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IRepository<Webhook> _webhookRepository;
    private readonly IRepository<ActivityLog> _activityLogRepository;

    public GetWebhookActivityLogsHandler(ILogger<GetWebhookActivityLogsHandler> logger, IMapper mapper,
        IRepository<Webhook> webhookRepository, IRepository<ActivityLog> activityLogRepository)
    {
        _logger = logger;
        _mapper = mapper;
        _webhookRepository = webhookRepository;
        _activityLogRepository = activityLogRepository;
    }

    public async Task<IPagedList<WebhookAcitvityLogModel>> Handle(GetWebhookLogsQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var webhook =
                await _webhookRepository.GetAsync(query => query.Where(webhook => webhook.Id == request.WebhookId));

            if (webhook == null)
                throw new WMSException($"Webhook not found! WebhookId: {request.WebhookId}", ExceptionType.NotFound,
                    HttpStatusCode.NotFound);

            var activityLogs = await GetWebhookActivityLogs(request);

            var webhooksList = activityLogs.Select(activityLog =>
                {
                    var result = _mapper.Map<WebhookAcitvityLogModel>(activityLog);
                    return result;
                })
                .ToList();

            return new PagedList<WebhookAcitvityLogModel>(webhooksList, activityLogs.PageIndex, activityLogs.PageSize,
                activityLogs.TotalCount);
        }
        catch (Exception ex)
        {
            if (ex is WMSException)
                throw;

            _logger.LogError(ex, "Failed to Get Webhooks! Message: {Message}", ex.Message);

            throw new WMSException("Failed to Get Webhooks!", ExceptionType.ServerError,
                HttpStatusCode.InternalServerError);
        }
    }

    private async Task<IPagedList<ActivityLog>> GetWebhookActivityLogs(GetWebhookLogsQuery request)
    {
        var query = _activityLogRepository.Table.Where(activityLog => !activityLog.IsDeleted);

        return await query.ToPagedListAsync(request.PageIndex, request.PageSize);
    }
}