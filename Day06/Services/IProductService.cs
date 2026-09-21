using Day06.Model;

namespace Day06.Services;

public interface IProductService
{
    List<Product> GetAll();
    Product? GetById(Guid id);
    Product Add(Product product);
    Product? UpdateById(Guid id,Product product);
    Product? DeleteById(Guid id);
}