using BookStore.Api.Data;
using BookStore.Api.Entities;
using BookStore.Shared.Dtos;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Api.Endpoints;

public static class OrderEndpoints
{
    public static void MapOrderEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/orders");

        // GET api/orders
        group.MapGet("/", async (BookStoreDbContext context) =>
        {
            var orders = await context.Orders
            .AsNoTracking()
            .Select(o => new OrderDto(
                o.Id,
                o.OrderDate,
                o.TotalPrice,
                o.StaffName,
                o.Items.Select(i => new OrderItemDto(
                    i.BookId,
                    i.Quantity,
                    i.UnitPrice
                )).ToList()
            ))
            .ToListAsync();

            return Results.Ok(orders);
        });
            
        // GET api/orders/{id}
        group.MapGet("/{id}", async (int id, BookStoreDbContext context) =>
        {
            var order = await context.Orders.FindAsync(id);

            if(order is null)
            {
                return Results.NotFound();
            }

            var orderDto = new OrderDto(
                order.Id,
                order.OrderDate,
                order.TotalPrice,
                order.StaffName,
                order.Items.Select(i => new OrderItemDto(
                    i.BookId,
                    i.Quantity,
                    i.UnitPrice
                )).ToList()
            );

            return Results.Ok(orderDto);
        })
        .WithName("GetOrderById");

        // POST api/orders/OrderDto
        group.MapPost("/", async (CreateOrderDto dto, BookStoreDbContext context) =>
        {
            if(dto.Items == null || dto.Items.Count == 0)
            {
                return Results.BadRequest("Order must contain at least one item.");
            }

            // Begin transaction
            using var transaction = await context.Database.BeginTransactionAsync();

            try
            {
                decimal calculatedTotal = 0M;

                var order = new Order
                {
                    StaffName = dto.StaffName,
                    Items = new List<OrderItem>()
                };

                var responseItems = new List<OrderItemDto>();

                foreach (var item in dto.Items)
                {
                    // Fetch book from database
                    var book = await context.Books.FindAsync(item.BookId);

                    if(book is null)
                    {
                        return Results.BadRequest($"Book with ID:{item.BookId} was not found");
                    }

                    // Check stock
                    if(book.StockQty < item.Quantity)
                    {
                        return Results.BadRequest($"Not enough '{book.Title}' in stock. Avaliable: {book.StockQty}. Request: {item.Quantity}."
                        ); 
                    }

                    // Deduct stock and add calculate total price
                    book.StockQty -= item.Quantity;

                    // Snapshot price
                    decimal unitPrice = book.Price;
                    calculatedTotal += book.Price * item.Quantity;

                    // Create new OrderItem
                    var orderItem = new OrderItem
                    {
                        BookId = book.Id,
                        Quantity = item.Quantity,
                        UnitPrice = unitPrice
                    };

                    order.Items.Add(orderItem);

                    // Create new response order item dto
                    responseItems.Add(new OrderItemDto(
                        book.Id,
                        item.Quantity,
                        unitPrice
                    ));
                }

                // Add property to Order
                order.TotalPrice = calculatedTotal;

                // Add Order
                context.Orders.Add(order);

                // Save change to database and commit
                await context.SaveChangesAsync();
                await transaction.CommitAsync();

                // Create response Dto
                var responseDto = new OrderDto(
                    order.Id,
                    order.OrderDate,
                    order.TotalPrice,
                    order.StaffName,
                    responseItems
                );

                return Results.CreatedAtRoute("GetOrderById", new {id = order.Id}, responseDto);

            }
            catch (Exception ex)
            {
                // Rollback all change
                await transaction.RollbackAsync();
                return Results.Problem($"Error occured while processing order. Message: {ex.Message}");
            }
        });
    }
}
