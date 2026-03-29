using Application.DTOs;
using MediatR;

namespace Application.Requests;

public class GetAllProductsRequest : IRequest<List<ProductDto>>;