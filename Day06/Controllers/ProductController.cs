using Microsoft.AspNetCore.Mvc;
using Day06.DTOs.RequestDto;
using Day06.Model;
using Day06.DTOs.ResponseDto;
using Day06.Services;
namespace Day06.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController(IProductService productService) : ControllerBase
{
    private readonly IProductService _productService = productService;

    [HttpGet]
    public IActionResult GetAll()
    {
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
        Product? product = _productService.GetById(id);
        if (product == null)
        {
            return NotFound(
                $"Product with id - {id} was not found in the list"
            );
        }
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
        if (string.IsNullOrWhiteSpace(productDto.Name))
        {
            return BadRequest(
                "Product name cannot be empty"
            );
        }
        if (productDto.Price <= 0)
        {
            return BadRequest(
                "Product price must be greater than zero"
            );
        }
        if (productDto.Stocks < 0)
        {
            return BadRequest(
                "Product stock cannot be negative"
            );
        }
        Product product = new()
        {
            Name = productDto.Name,
            Price = productDto.Price,
            Stocks = productDto.Stocks
        };

        _productService.Add(product);

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

        if (string.IsNullOrWhiteSpace(requestProductDto.Name))
        {
            return BadRequest(
                "Product name cannot be empty"
            );
        }
        if (requestProductDto.Price <= 0)
        {
            return BadRequest(
                "Product price must be greater than zero"
            );
        }
        if (requestProductDto.Stocks < 0)
        {
            return BadRequest(
                "Product stock cannot be negative"
            );
        }

        Product product1 = new()
        {
            Name = requestProductDto.Name,
            Price = requestProductDto.Price,
            Stocks = requestProductDto.Stocks
        };

        Product? product2 = _productService.UpdateById(id, product1);

        if (product2 is null)
        {
            return NotFound(
                $"Product with ID: {id} does not exist in the list to update"
            );
        }

        ResponseProductDto responseProductDto = new(
            product2.Id,
            product2.Name,
            product2.Price,
            product2.Stocks
        );

        return Ok(responseProductDto);
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteById(Guid id)
    {
        Product? deletedProduct = _productService.DeleteById(id);
        if (deletedProduct is null)
        {
            return NotFound(
                $"Product with ID: {id} does not exist in the list to delete"
            );
        }

        ResponseProductDto responseProductDto = new(
            deletedProduct.Id,
            deletedProduct.Name,
            deletedProduct.Price,
            deletedProduct.Stocks
        );
        return Ok(responseProductDto);
    }
}
