using System.ComponentModel.DataAnnotations;

namespace BookStore.Shared.Dtos;

public record CreateBookDto(

    [Required(ErrorMessage = "Title is required")]
    [StringLength(150, ErrorMessage ="Title cannot exceed 150 characters")]
    string Title,

    [Required(ErrorMessage = "Author name is required")]
    [StringLength(100, ErrorMessage = "Author cannot exceed 100 characters")]
    string Author,

    [Range(0.01, 10000.00, ErrorMessage = "Price must be greater than 0")]
    decimal Price,

    [Range(0, 10000, ErrorMessage = "Stock quantity cannot be negative")]
    int StockQty
);

