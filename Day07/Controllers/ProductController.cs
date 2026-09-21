using Microsoft.AspNetCore.Mvc;
using Day07.DTOs.RequestDto;
using Day07.Model;
using Day07.DTOs.ResponseDto;
using Day07.Services;
namespace Day07.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController(IProductService productService, ILogger<ProductController> logger) : ControllerBase
{
    private readonly IProductService _productService = productService;
    private readonly ILogger<ProductController> _logger = logger;

    [HttpGet]
    public IActionResult GetAll()
    {
        _logger.LogInformation(
            "Getting all products"
        );

        List<ResponseProductDto> response = [.. _productService.GetAll()
        .Select(product =>
        new ResponseProductDto(
            product.Id,
            product.Name,
            product.Price,
            product.Stocks
        ))];

        return Ok(
            response
        );
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetProductById(Guid id)
    {
        _logger.LogInformation(
            "Getting product with ID {ProductId}", id
        );

        Product product = _productService.GetById(id);

        ResponseProductDto response = new(
            product.Id,
            product.Name,
            product.Price,
            product.Stocks
        );
        return Ok(response);
    }
    [HttpPost]
    public IActionResult CreateProduct([FromBody] RequestProductDto productDto)
    {
        Product product = new()
        {
            Name = productDto.Name,
            Price = productDto.Price,
            Stocks = productDto.Stocks
        };

        _productService.Add(product);

        _logger.LogInformation(
            "Product {ProductId} Id created successfully",product.Id
        );

        ResponseProductDto responseProductDto = new(
            product.Id,
            product.Name,
            product.Price,
            product.Stocks
        );

        return CreatedAtAction(
            nameof(GetProductById),
            new { id = product.Id },
            responseProductDto
        );
    }

    [HttpPut("{id:guid}")]
    public IActionResult UpdateById(Guid id, [FromBody] RequestProductDto requestProductDto)
    {
        Product product1 = new()
        {
            Name = requestProductDto.Name,
            Price = requestProductDto.Price,
            Stocks = requestProductDto.Stocks
        };

        Product product2 = _productService.UpdateById(id, product1);

        ResponseProductDto responseProductDto = new(
            product2.Id,
            product2.Name,
            product2.Price,
            product2.Stocks
        );

        _logger.LogInformation(
            "Product with {ProductId} Id updated successfully", responseProductDto.Id
        );

        return Ok(responseProductDto);
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteById(Guid id)
    {
        Product deletedProduct = _productService.DeleteById(id);

        ResponseProductDto responseProductDto = new(
            deletedProduct.Id,
            deletedProduct.Name,
            deletedProduct.Price,
            deletedProduct.Stocks
        );

        _logger.LogInformation(
            "Product with {ProductId} Id deleted successfully", responseProductDto.Id
        );

        return Ok(responseProductDto);
    }
}