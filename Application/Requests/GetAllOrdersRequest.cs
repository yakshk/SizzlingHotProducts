using Application.DTOs;
using MediatR;

namespace Application.Requests;

public class GetAllOrdersRequest : IRequest<List<OrderDto>>;