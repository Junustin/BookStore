using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Api.Entities;

public class Order
{
    public int Id {get; set;} // PK
    public DateTime OrderDate { get; set; } = DateTime.Now;
    public decimal TotalPrice { get; set; }
    public string StaffName { get; set; } = string.Empty;

    // Navigation property
    public List<OrderItem> Items {get; set;} = [];
}