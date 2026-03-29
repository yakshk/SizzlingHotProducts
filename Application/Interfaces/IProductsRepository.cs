using Application.Models;

namespace Application.Interfaces;

public interface IProductsRepository
{
    Task<List<Product>> GetAllAsync();
}