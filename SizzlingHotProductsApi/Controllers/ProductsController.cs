using Application.DTOs;
using Application.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SizzlingHotProductsApi.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController(IMediator mediator) : ControllerBase
{
    [HttpGet("get-all")]
    public async Task<List<ProductDto>> GetAll()
    {
        var result = await mediator.Send(new GetAllProductsRequest());
        return result;
    }
}