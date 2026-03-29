using Application.DTOs;
using Application.Extensions;
using Application.Interfaces;
using Application.Requests;
using MediatR;

namespace Application.Handlers;

public class GetAllOrdersHandler(IOrdersRepository repo) : IRequestHandler<GetAllOrdersRequest, List<OrderDto>>
{
    public async Task<List<OrderDto>> Handle(GetAllOrdersRequest request, CancellationToken cancellationToken)
    {
        var orders = await repo.GetAllAsync();
        return orders.Select(x => x.ToDto()).ToList();
    }
}