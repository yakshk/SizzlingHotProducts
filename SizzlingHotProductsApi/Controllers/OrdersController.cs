using Application.DTOs;
using Application.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SizzlingHotProductsApi.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController(IMediator mediator) : ControllerBase
{
    [HttpGet("get-all")]
    public async Task<List<OrderDto>> GetAll()
    {
        var result = await mediator.Send(new GetAllOrdersRequest());
        return result;
    }
}