using System.ComponentModel.DataAnnotations;

namespace BookStore.Shared.Dtos;

public record UpdateBookDto(

    [Required(ErrorMessage = "Title is required")]
    [StringLength(150)]
    string Title,

    [Required(ErrorMessage = "Author name is required")]
    [StringLength(100)]
    string Author,

    [Range(0.01, 10000.00)]
    decimal Price,

    [Range(0, 10000)]
    int StockQty
);


