using Application.Constants;
using Application.DTOs;
using Application.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SizzlingHotProductsApi.Controllers;

[ApiController]
[Route("api/hot-products")]
public class HotProductsController(IMediator mediator) : ControllerBase
{
    [HttpGet("today")]
    public async Task<HotProductDto> HotProductToday()
    {
        // Adjustment for the date assumption stated in the README.
        // If this was something permanent, a clock abstraction should be injected instead.
        var request = new HotProductRequest
        {
            StartDate = BusinessConstants.Today,
            EndDate = BusinessConstants.Today
        };

        var result = await mediator.Send(request);
        return result;
    }

    [HttpGet("past-three-days")]
    public async Task<HotProductDto> HotProductPastThreeDays()
    {
        // Adjustment for the date assumption stated in the README.
        // If this was something permanent, a clock abstraction should be injected instead.
        var request = new HotProductRequest
        {
            StartDate = BusinessConstants.Today.AddDays(-2),
            EndDate = BusinessConstants.Today
        };

        var result = await mediator.Send(request);
        return result;
    }

    [HttpPost("date-range")]
    public async Task<HotProductDto> DateRange([FromBody] HotProductRequest request)
    {
        var result = await mediator.Send(request);
        return result;
    }
}