using System.ComponentModel.DataAnnotations;

namespace BookStore.Shared;

// Use in CreateOrderDto only care about Book id and quantity to buy
public record OrderItemRequestDto(
    int BookId,
    [Range(1, 1000, ErrorMessage = "Quantity must be at least 1")]
    int Quantity
);

