namespace BookStore.Shared.Dtos;

public record OrderDto(
    int Id,
    DateTime OrderDate,
    decimal TotalPrice,
    string StaffName,
    // BookId, Quantiy, UnitPrice
    List<OrderItemDto> OrderItems
);
