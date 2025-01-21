using System.Net;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using WMS.Core.Exceptions;
using WMS.Core.Repositories;
using WMS.Models.Entities;
using WMS.Models.Enums;
using WMS.Models.Webhooks.v1.Commands.DeleteWebhook;
using WMS.Models.Webhooks.v1.Shared;

namespace WMS.Core.Handlers.Webhooks.v1.Commands;

public class DeleteWebhookHandler : IRequestHandler<DeleteWebhookCommand, DeleteWebhookResponse>
{
    private readonly ILogger<DeleteWebhookHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IRepository<Webhook> _webhookRepository;

    public DeleteWebhookHandler(ILogger<DeleteWebhookHandler> logger, IMapper mapper, IRepository<Webhook> webhookRepository)
    {
        _logger = logger;
        _mapper = mapper;
        _webhookRepository = webhookRepository;
    }

    public async Task<DeleteWebhookResponse> Handle(DeleteWebhookCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var webhook = await _webhookRepository.GetAsync(query => query.Where(webhook => webhook.Id == request.Id));

            if (webhook == null)
                throw new WMSException("Webhook not found! WebhookId: {request.Id}", ExceptionType.WebhookNotFound,
                                       HttpStatusCode.NotFound);

            await _webhookRepository.DeleteAsync(webhook);

            return new DeleteWebhookResponse
            {
                Deleted = true,
                Webhook = _mapper.Map<WebhookModel>(webhook)
            };
        }
        catch (Exception ex)
        {
            if (ex is WMSException)
                throw;
            
            _logger.LogError(ex, "Failed to Delete Webhook! WebhookId: {WebhookId}, Message: {Message}", request.Id, ex.Message);

            throw new WMSException("Failed to Delete Webhook!", ExceptionType.NotDeleted, HttpStatusCode.InternalServerError);
        }
    }
}