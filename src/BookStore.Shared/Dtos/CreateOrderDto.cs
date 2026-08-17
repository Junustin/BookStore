using System.ComponentModel.DataAnnotations;

namespace BookStore.Shared;

public record CreateOrderDto(

    [Required(ErrorMessage = "Staff name is required")]
    string StaffName,
    
    List<OrderItemRequestDto> Items
);

