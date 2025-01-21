using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WMS.Api.Controllers.Base;
using WMS.Core.Extensions;
using WMS.Core.Services.IServices;
using WMS.Models.Common;
using WMS.Models.Common.Pagination;
using WMS.Models.Entities;
using WMS.Models.Orders.v1.Commands.CreateOrder;
using WMS.Models.Orders.v1.Commands.DeleteOrder;
using WMS.Models.Orders.v1.Commands.UpdateOrder;
using WMS.Models.Orders.v1.Queries.GetOrder;
using WMS.Models.Orders.v1.Queries.GetOrders;
using WMS.Models.Orders.v1.Shared;

namespace WMS.Api.Controllers.v1;

[Route("api/v1/orders")]
public class OrderController : BaseController
{
    private readonly IPublisherService _publisherService;

    public OrderController(IMediator mediator, IPublisherService publisherService) : base(mediator)
    {
        _publisherService = publisherService;
    }

    [HttpGet("{orderId}")]
    public async Task<IActionResult> GetOrderAsync(Guid orderId)
    {
        var response = new ResponseModel<OrderModel>();

        if (!ModelState.IsValid)
        {
            response.AddErrors(ModelState.GetErrorMessages());

            return BadRequest(response.BadRequest());
        }

        var getOrderQuery = new GetOrderQuery
        {
            Id = orderId
        };

        var result = await Mediator.Send(getOrderQuery);

        return Ok(response.Ok(result));
    }

    [HttpGet]
    public async Task<IActionResult> GetOrdersAsync([FromQuery] GetOrdersQuery request)
    {
        var response = new ResponseModel<IPagedList<OrderListModel>>();

        if (!ModelState.IsValid)
        {
            response.AddErrors(ModelState.GetErrorMessages());

            return BadRequest(response.BadRequest());
        }

        var getOrdersResult = await Mediator.Send(request);

        return Ok(response.Ok(getOrdersResult,
                              new PaginationResultInfo
                              {
                                  PageIndex = getOrdersResult.PageIndex,
                                  PageSize = getOrdersResult.PageSize,
                                  TotalCount = getOrdersResult.TotalCount,
                                  TotalPages = getOrdersResult.TotalPages
                              }));
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateOrderAsync([FromBody] CreateOrderCommand request)
    {
        var response = new ResponseModel<OrderModel>();

        if (!ModelState.IsValid)
        {
            response.AddErrors(ModelState.GetErrorMessages());

            return BadRequest(response.BadRequest());
        }

        var result = await Mediator.Send(request);

        return Ok(response.Ok(result));
    }

    [HttpPut("{orderId}/update")]
    public async Task<IActionResult> UpdateOrderAsync(Guid orderId, [FromBody] UpdateOrderCommand request)
    {
        var response = new ResponseModel<UpdateOrderResponse>();

        if (!ModelState.IsValid)
        {
            response.AddErrors(ModelState.GetErrorMessages());

            return BadRequest(response.BadRequest());
        }

        request.Id = orderId;

        var result = await Mediator.Send(request);

        return Ok(response.Ok(result));
    }

    [HttpDelete("{orderId}/delete")]
    public async Task<IActionResult> DeleteOrderAsync(Guid orderId)
    {
        var response = new ResponseModel<DeleteOrderResponse>();

        if (!ModelState.IsValid)
        {
            response.AddErrors(ModelState.GetErrorMessages());

            return BadRequest(response.BadRequest());
        }

        var deleteOrderCommand = new DeleteOrderCommand
        {
            Id = orderId
        };

        var result = await Mediator.Send(deleteOrderCommand);

        return Ok(response.Ok(result));
    }
}