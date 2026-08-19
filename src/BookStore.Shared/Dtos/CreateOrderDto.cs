using System.ComponentModel.DataAnnotations;

namespace BookStore.Shared.Dtos;

// Use to create Order in endpoint
public record CreateOrderDto(

    [Required(ErrorMessage = "Staff name is required")]
    string StaffName,
    
    // Book id and Quantity
    List<OrderItemRequestDto> Items
);

