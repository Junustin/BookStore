namespace BookStore.Shared.Dtos;

public record OrderDto(
    int Id,
    DateTime OrderDate,
    decimal TotalPrice,
    string StaffName,
    List<OrderItemDto> OrderItems
);

// Use to display detail of book in order detail (receipt)
public record OrderItemDto(
    int BookId,
    int Quantity,
    decimal UnitPrice
);
