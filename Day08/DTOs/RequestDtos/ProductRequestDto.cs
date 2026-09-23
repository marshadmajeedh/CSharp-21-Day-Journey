using System.ComponentModel.DataAnnotations;

namespace Day08.DTOs.RequestDtos;

public record ProductRequestDto(
    [Required]
    [StringLength(
        100,
        MinimumLength = 2,
        ErrorMessage = "Name must have characters more than 1 and less than 101"
    )]
    string Name,

    [Range(
        0.01,
        double.MaxValue,
        ErrorMessage = "Price must be greater than 0"
    )]
    decimal Price,

    [Range(
        0,
        int.MaxValue,
        ErrorMessage = "Stocks cannot be negative"
    )]
    int Stocks
);