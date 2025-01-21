using System.Net;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using WMS.Core.Exceptions;
using WMS.Core.Extensions;
using WMS.Core.Repositories;
using WMS.Models.Common;
using WMS.Models.Common.Pagination;
using WMS.Models.Entities;
using WMS.Models.Enums;
using WMS.Models.Webhooks.v1.Queries.GetWebhooks;

namespace WMS.Core.Handlers.Webhooks.v1.Queries;

public class GetWebhooksHandler : IRequestHandler<GetWebhooksQuery, IPagedList<WebhookListModel>>
{
    private readonly ILogger<GetWebhooksHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IRepository<Webhook> _webhookRepository;

    public GetWebhooksHandler(ILogger<GetWebhooksHandler> logger, IMapper mapper,
                              IRepository<Webhook> webhookRepository)
    {
        _logger = logger;
        _mapper = mapper;
        _webhookRepository = webhookRepository;
    }

    public async Task<IPagedList<WebhookListModel>> Handle(GetWebhooksQuery request,
                                                           CancellationToken cancellationToken)
    {
        try
        {
            var webhooks = await GetWebhooks(request);

            var webhooksList = webhooks.Select(order =>
                                       {
                                           var result = _mapper.Map<WebhookListModel>(order);
                                           return result;
                                       })
                                       .ToList();

            return new PagedList<WebhookListModel>(webhooksList, webhooks.PageIndex, webhooks.PageSize,
                                                   webhooks.TotalCount);
        }
        catch (Exception ex)
        {
            if (ex is WMSException)
                throw;
            
            _logger.LogError(ex, "Failed to Get Webhooks! Message: {Message}", ex.Message);

            throw new WMSException("Failed to Get Webhooks!", ExceptionType.ServerError, HttpStatusCode.InternalServerError);
        }
    }

    private async Task<IPagedList<Webhook>> GetWebhooks(GetWebhooksQuery request)
    {
        var query = _webhookRepository.Table.Where(webhook => !webhook.IsDeleted);

        query = query.GetOrderedEntities("Id", request.OrderBy, request.SortingOrder ?? SortingOrder.DESC);

        return await query.ToPagedListAsync(request.PageIndex, request.PageSize);
    }
}