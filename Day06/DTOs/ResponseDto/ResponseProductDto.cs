namespace Day06.DTOs.ResponseDto;

public record ResponseProductDto(
    Guid Id,
    string Name,
    decimal Price,
    int Stocks
);