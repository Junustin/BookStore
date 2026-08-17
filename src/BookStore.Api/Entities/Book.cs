using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Api.Entities;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int StockQty { get; set; }
    
    public DateTime CreateAt { get; set; } = DateTime.Now;
}
