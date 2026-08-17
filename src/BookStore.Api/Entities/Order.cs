namespace BookStore.Api.Entities;

public class Order
{
    public int Id {get; set;}
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public decimal TotalPrice { get; set; }
    public string StaffName { get; set; } = string.Empty;
}
