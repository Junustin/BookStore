using BookStore.Api.Endpoints;
using BookStore.Api.Data;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Register OpenAPI
builder.Services.AddOpenApi();

// Get Connection string frrom appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Register DbContext with SQLite provider to Dependency injection
builder.Services.AddDbContext<BookStoreDbContext>(options =>
    options.UseSqlite(connectionString));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapBookEndpoints();
app.MapOrderEndpoints();

app.UseHttpsRedirection();

app.Run();