namespace BookStore.Shared;

public record OrderDto(
    int Id,
    DateTime OrderDate,
    decimal TotalPrice,
    string StaffName
);

