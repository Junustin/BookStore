namespace BookStore.Shared.Dtos;

public record BookDto
(
    int Id,
    string Title,
    string Author,
    decimal Price,
    int StockQty,
    DateTime CreatedAt
);