namespace Day07.DTOs.ResponseDto;

public record ResponseProductDto(
    Guid Id,
    string Name,
    decimal Price,
    int Stocks
);