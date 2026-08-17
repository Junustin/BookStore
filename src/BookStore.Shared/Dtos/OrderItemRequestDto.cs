using System.ComponentModel.DataAnnotations;

namespace BookStore.Shared;

public record OrderItemRequestDto(
    int BookId,
    [Range(1, 1000, ErrorMessage = "Quantity must be at least 1")]
    int Quantity
);

