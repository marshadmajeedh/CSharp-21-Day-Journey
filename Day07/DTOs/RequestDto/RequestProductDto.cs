using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace Day07.DTOs.RequestDto;

public class RequestProductDto
{
    [Required]
    [StringLength(
        100,
        MinimumLength = 2,
        ErrorMessage = "Product name's length must be greater than 1 and less than 101"
    )]

    public string Name { get; set; } = "";
    [Range(
        0.01,
        double.MaxValue,
        ErrorMessage = "Price must be greater than Zero"
    )]
    public decimal Price { get; set; }

    [Range(
        0,
        int.MaxValue,
        ErrorMessage = "Stock cannot be negative"
    )]
    public int Stocks{ get; set; }
}