using System.ComponentModel.DataAnnotations;

namespace BookStore.Shared;

public record CreateOrderDto(

    [Required(ErrorMessage = "Staff name is required")]
    string StaffName,
    
    [Range(0.01, 100000.00, ErrorMessage = "Total price must be greater than 0")]
    decimal TotalPrice
);

