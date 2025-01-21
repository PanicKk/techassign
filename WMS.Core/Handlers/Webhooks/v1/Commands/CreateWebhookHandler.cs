using System.Net;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using WMS.Core.Exceptions;
using WMS.Core.Repositories;
using WMS.Models.Entities;
using WMS.Models.Enums;
using WMS.Models.Webhooks.v1.Commands.CreateWebhook;
using WMS.Models.Webhooks.v1.Shared;

namespace WMS.Core.Handlers.Webhooks.v1.Commands;

public class CreateWebhookHandler : IRequestHandler<CreateWebhookCommand, WebhookModel>
{
    private readonly ILogger<CreateWebhookHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IRepository<Webhook> _webhookRepository;

    public CreateWebhookHandler(ILogger<CreateWebhookHandler> logger, IMapper mapper,
                                IRepository<Webhook> webhookRepository)
    {
        _logger = logger;
        _mapper = mapper;
        _webhookRepository = webhookRepository;
    }

    public async Task<WebhookModel> Handle(CreateWebhookCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var webhook = _mapper.Map<Webhook>(request);

            await _webhookRepository.InsertAsync(webhook);

            return _mapper.Map<WebhookModel>(webhook);
        }
        catch (Exception ex)
        {
            if (ex is WMSException)
                throw;
            
            _logger.LogError(ex, "Failed to Create Webhook! Message: {Message}", ex.Message);

            throw new WMSException("Failed to Create Webhook!", ExceptionType.NotCreated, HttpStatusCode.InternalServerError);
        }
    }
}