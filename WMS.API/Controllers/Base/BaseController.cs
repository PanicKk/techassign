using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WMS.Api.Controllers.Base;

[ApiController]
[Route("api/[controller]")]
public class BaseController : ControllerBase
{
    private readonly IMediator _mediator;

    protected IMediator Mediator => _mediator;

    public BaseController(IMediator mediator)
    {
        _mediator = mediator;
    }
}