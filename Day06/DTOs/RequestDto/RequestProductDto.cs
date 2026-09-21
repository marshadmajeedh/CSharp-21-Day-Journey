namespace Day06.DTOs.RequestDto;

public record RequestProductDto
(
    string Name,
    decimal Price,
    int Stocks
);