using Day07.Exceptions;
using Day07.Model;
namespace Day07.Services;
public class ProductService : IProductService
{
    private readonly List<Product> _products = [];

    public List<Product> GetAll()
    {
        return _products;
    }

    public Product Add(Product product)
    {
        _products.Add(product);

        return product;
    }

    public Product GetById(Guid id)
    {
        Product product = _products.FirstOrDefault(
            product => product.Id == id
        ) ?? throw new ProductNotFoundException(id);
        return product;
    }

    public Product UpdateById(Guid id,Product product1)
    {
        Product product = GetById(id);

        product.Name = product1.Name;
        product.Price = product1.Price;
        product.Stocks = product1.Stocks;

        return product;
    }

    public Product DeleteById(Guid id)
    {
        Product product = GetById(id);

        _products.Remove(product);

        return product;
    }
}