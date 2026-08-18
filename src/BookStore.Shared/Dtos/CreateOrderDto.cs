using System.ComponentModel.DataAnnotations;

namespace BookStore.Shared.Dtos;

public record CreateOrderDto(

    [Required(ErrorMessage = "Staff name is required")]
    string StaffName,
    
    List<OrderItemRequestDto> Items
);

