using MediatR;
using Microsoft.AspNetCore.Mvc;
using WMS.Api.Controllers.Base;
using WMS.Core.Extensions;
using WMS.Models.Common;
using WMS.Models.Common.Pagination;
using WMS.Models.Webhooks.v1.Commands.CreateWebhook;
using WMS.Models.Webhooks.v1.Commands.DeleteWebhook;
using WMS.Models.Webhooks.v1.Commands.UpdateWebhook;
using WMS.Models.Webhooks.v1.Queries.GetWebhook;
using WMS.Models.Webhooks.v1.Queries.GetWebhooks;
using WMS.Models.Webhooks.v1.Shared;

namespace WMS.Api.Controllers.v1;

[Route("api/v1/webhooks")]
public class WebhookController : BaseController
{
    public WebhookController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet("{webhookId}")]
    public async Task<IActionResult> GetWebhookAsync(Guid webhookId)
    {
        var response = new ResponseModel<WebhookModel>();

        if (!ModelState.IsValid)
        {
            response.AddErrors(ModelState.GetErrorMessages());

            return BadRequest(response.BadRequest());
        }

        var getWebhookQuery = new GetWebhookQuery
        {
            Id = webhookId
        };

        var result = await Mediator.Send(getWebhookQuery);

        return Ok(response.Ok(result));
    }

    [HttpGet]
    public async Task<IActionResult> GetWebhooksAsync([FromQuery] GetWebhooksQuery request)
    {
        var response = new ResponseModel<IPagedList<WebhookListModel>>();

        if (!ModelState.IsValid)
        {
            response.AddErrors(ModelState.GetErrorMessages());

            return BadRequest(response.BadRequest());
        }

        var getWebhooksResult = await Mediator.Send(request);

        return Ok(response.Ok(getWebhooksResult,
                              new PaginationResultInfo
                              {
                                  PageIndex = getWebhooksResult.PageIndex,
                                  PageSize = getWebhooksResult.PageSize,
                                  TotalCount = getWebhooksResult.TotalCount,
                                  TotalPages = getWebhooksResult.TotalPages
                              }));
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateWebhookAsync([FromBody] CreateWebhookCommand request)
    {
        var response = new ResponseModel<WebhookModel>();

        if (!ModelState.IsValid)
        {
            response.AddErrors(ModelState.GetErrorMessages());

            return BadRequest(response.BadRequest());
        }

        var result = await Mediator.Send(request);

        return Ok(response.Ok(result));
    }

    [HttpPut("{webhookId}/update")]
    public async Task<IActionResult> UpdateWebhookAsync(Guid webhookId, [FromBody] UpdateWebhookCommand request)
    {
        var response = new ResponseModel<UpdateWebhookResponse>();

        if (!ModelState.IsValid)
        {
            response.AddErrors(ModelState.GetErrorMessages());

            return BadRequest(response.BadRequest());
        }

        request.Id = webhookId;

        var result = await Mediator.Send(request);

        return Ok(response.Ok(result));
    }

    [HttpDelete("{webhookId}/delete")]
    public async Task<IActionResult> DeleteWebhookAsync(Guid webhookId)
    {
        var response = new ResponseModel<DeleteWebhookResponse>();

        if (!ModelState.IsValid)
        {
            response.AddErrors(ModelState.GetErrorMessages());

            return BadRequest(response.BadRequest());
        }

        var deleteWebhookCommand = new DeleteWebhookCommand
        {
            Id = webhookId
        };

        var result = await Mediator.Send(deleteWebhookCommand);


        return Ok(response.Ok(result));
    }
}