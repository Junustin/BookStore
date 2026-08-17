using BookStore.Api.Data;
using BookStore.Api.Entities;
using BookStore.Shared.Dtos;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Api.Endpoints;

public static class BookEndpoints
{
    public static void MapBookEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/books");

        // GET /api/books
        group.MapGet("/", async (BookStoreDbContext context) =>
        {
            return await context.Books
           .AsNoTracking()
           .Select(b => new BookDto(
               b.Id,
               b.Title,
               b.Author,
               b.Price,
               b.StockQty,
               b.CreateAt
           ))
           .ToListAsync();
        });

        // GET /api/books/{id}
        group.MapGet("/{id}", async (int id, BookStoreDbContext context) =>
        {
            var book = await context.Books.FindAsync(id);

            if(book is null)
            {
                return Results.NotFound($"Book with ID:{id} was not found");
            }
            // Create BookDto to return
            BookDto bookDto = new BookDto(
                book.Id,
                book.Title,
                book.Author,
                book.Price,
                book.StockQty,
                book.CreateAt 
            );

            return Results.Ok(bookDto);
        })
        .WithName("GetBookById");

        // POST /api/books/BookDto
        group.MapPost("/", async (CreateBookDto dto, BookStoreDbContext context) =>
        {
            var book = new Book
            {
                Title = dto.Title,
                Author = dto.Author,
                Price = dto.Price,
                StockQty = dto.StockQty,
            };

            context.Books.Add(book);
            await context.SaveChangesAsync();

            // Create new BookDto to response
            var responseDto = new BookDto(
                book.Id,
                book.Title,
                book.Author,
                book.Price,
                book.StockQty,
                book.CreateAt
            );

            return Results.CreatedAtRoute("GetBookById", new{id = book.Id},responseDto);
        });

        // PUT /api/books/{id}
        group.MapPut("/{id}", async (int id, UpdateBookDto dto, BookStoreDbContext context) =>
        {
            // Fetch book Ref to update from Id
            var existingBook = await context.Books.FindAsync(id);

            if(existingBook is null)
            {
                return Results.NotFound($"Book with ID:{id} not found");
            }

            existingBook.Title = dto.Title;
            existingBook.Author = dto.Author;
            existingBook.Price = dto.Price;
            existingBook.StockQty = dto.StockQty;

            await context.SaveChangesAsync();

            return Results.NoContent();
        });

        // DELETE /api/books/{id}
        group.MapDelete("/{id}", async (int id, BookStoreDbContext context) =>
        {
            await context.Books
                        .Where(b => b.Id == id)
                        .ExecuteDeleteAsync();

            return Results.NoContent();
        });
    }
}
