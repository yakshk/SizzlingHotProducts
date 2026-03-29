using Application.Models;

namespace Application.Interfaces;

public interface IOrdersRepository
{
    Task<List<Order>> GetAllAsync();
}