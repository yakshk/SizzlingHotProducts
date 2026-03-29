using Application.DTOs;
using Application.Extensions;
using Application.Interfaces;
using Application.Requests;
using MediatR;

namespace Application.Handlers;

public class GetProductsHandler(IProductsRepository repo) : IRequestHandler<GetAllProductsRequest, List<ProductDto>>
{
    public async Task<List<ProductDto>> Handle(GetAllProductsRequest request, CancellationToken cancellationToken)
    {
        var products = await repo.GetAllAsync();
        return products.Select(x => x.ToDto()).ToList();
    }
}