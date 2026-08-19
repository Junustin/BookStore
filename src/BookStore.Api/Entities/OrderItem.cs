namespace BookStore.Api.Entities;

public class OrderItem
{
    public int Id {get;set;}

    // FK
    public int OrderId{get;set;} //FK to Orders table
    public int BookId {get; set;} //FK to Books table
    // Line Data
    public int Quantity{get;set;}
    public decimal UnitPrice{get; set;}

    public Order Order {get; set;} = null!;
    public Book Book {get;set;} = null!;
}