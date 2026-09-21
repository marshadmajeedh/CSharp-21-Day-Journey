using Day06.Model;
namespace Day06.Services;
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

    public Product? GetById(Guid Id)
    {
        return _products.FirstOrDefault(
            product => product.Id == Id
        );
    }

    public Product? UpdateById(Guid Id,Product product1)
    {
        Product? product = GetById(Id);

        if (product is null)
        {
            return null;
        }

        product.Name = product1.Name;
        product.Price = product1.Price;
        product.Stocks = product1.Stocks;

        return product;
    }

    public Product? DeleteById(Guid Id)
    {
        Product? product = GetById(Id);
        if (product is null)
        {
            return null;
        }
        _products.Remove(product);

        return product;
    }
}