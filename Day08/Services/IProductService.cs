using Day08.Model;
using Microsoft.EntityFrameworkCore;

namespace Day08.Services;

public interface IProductService
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task <Product> GetByIdAsync(Guid id);
    Task <Product> AddProductAsync(Product product);
    Task <Product> UpdateByIdAsync(Guid id, Product product);
    Task <Product> DeleteByIdAsync(Guid id);
}