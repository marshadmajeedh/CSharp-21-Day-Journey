using Day08.Services;
using Microsoft.AspNetCore.Mvc;
using Day08.Model;
using Day08.DTOs.ResponseDtos;
using System.Linq;
using Day08.DTOs.RequestDtos;
using System.Threading.Tasks;
namespace Day08.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class ProductController(IProductService productService, ILogger<ProductController> logger) : ControllerBase
{
    private readonly IProductService _productService = productService;
    private readonly ILogger<ProductController> _logger = logger;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation(
            "Getting all products"
        );

        var products = await _productService.GetAllAsync();
        List<ProductResponseDto> response = [..products.Select(product => new ProductResponseDto(
            product.Id,
            product.Name,
            product.Price,
            product.Stocks
        ))];
        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        _logger.LogInformation(
            "Getting product with Id : {productId}", id
        );
        Product product = await _productService.GetByIdAsync(id);

        ProductResponseDto response = new(
            product.Id,
            product.Name,
            product.Price,
            product.Stocks
        );
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Add(ProductRequestDto productRequestDto)
    {
        Product product = new()
        {
            Name = productRequestDto.Name,
            Price = productRequestDto.Price,
            Stocks = productRequestDto.Stocks
        };

        await _productService.AddProductAsync(product);

        ProductResponseDto response= new(
            product.Id,
            product.Name,
            product.Price,
            product.Stocks
        );
        return CreatedAtAction(
            actionName: nameof(GetById),
            routeValues: new { id = response.Id },
            value: response
        );
    }
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateById(Guid id, [FromBody] ProductRequestDto productRequestDto)
    {
        Product product = new()
        {
            Name = productRequestDto.Name,
            Price = productRequestDto.Price,
            Stocks = productRequestDto.Stocks
        };

        Product updatedProduct = await _productService.UpdateByIdAsync(id, product);

        ProductResponseDto response = new(
            updatedProduct.Id,
            updatedProduct.Name,
            updatedProduct.Price,
            updatedProduct.Stocks
        );
        return Ok(response);
    }
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteById(Guid id)
    {
        Product product = await _productService.DeleteByIdAsync(id);

        ProductResponseDto response = new(
           product.Id,
           product.Name,
           product.Price,
           product.Stocks
        );
        return Ok(response);
    }
}
