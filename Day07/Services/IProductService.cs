using Day07.Model;

namespace Day07.Services;

public interface IProductService
{
    List<Product> GetAll();
    Product GetById(Guid id);
    Product Add(Product product);
    Product UpdateById(Guid id,Product product);
    Product DeleteById(Guid id);
}