namespace Day08.DTOs.ResponseDtos;

public record ProductResponseDto(
    Guid Id,
    string Name,
    decimal Price,
    int Stocks
);
