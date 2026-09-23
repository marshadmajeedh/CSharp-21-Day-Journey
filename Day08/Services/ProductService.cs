using Day08.Data;
using Day08.Exceptions;
using Day08.Model;
using Microsoft.EntityFrameworkCore;
namespace Day08.Services;

public class ProductService(AppDbContext context) : IProductService
{
    private readonly AppDbContext _context = context;

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products.AsNoTracking().ToListAsync();
    }

    public async Task<Product>  AddProductAsync(Product addedProduct)
    {
        _context.Products.Add(addedProduct);
        await _context.SaveChangesAsync();
        return addedProduct;
    }

    public async Task<Product> GetByIdAsync(Guid id)
    {
        Product product = await _context.Products.FirstOrDefaultAsync(product => product.Id == id) ?? throw new ProductNotFoundException(id);

        return product;
    }
    public async Task<Product> UpdateByIdAsync(Guid id,Product updatedProduct)
    {
        Product product = await GetByIdAsync(id);

        product.Name = updatedProduct.Name;
        product.Price = updatedProduct.Price;
        product.Stocks = updatedProduct.Stocks;

        await _context.SaveChangesAsync();

        return product;
    }
    public async Task<Product> DeleteByIdAsync(Guid id)
    {
        Product product = await GetByIdAsync(id);

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return product;
    }

}