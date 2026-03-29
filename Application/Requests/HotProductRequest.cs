using Application.DTOs;
using MediatR;

namespace Application.Requests;

public class HotProductRequest : IRequest<HotProductDto>
{
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
}