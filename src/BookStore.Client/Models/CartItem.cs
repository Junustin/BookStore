using System;

namespace BookStore.Client.Models;

public class CartItem
{
    public int BookId {get;set;}
    public string? Title{get;set;}
    public decimal Price {get;set;} // Snapshot of book price from database
    public int Quantity {get;set;}
    public int AvailableStock{get;set;}
    public decimal Subtotal => Price * Quantity;
}
