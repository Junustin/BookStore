namespace BookStore.Shared.Dtos;

// Use to display detail of book in order detail (receipt)
public record OrderItemDto(
    int BookId,
    string BookTitle,
    int Quantity,
    decimal UnitPrice
);
