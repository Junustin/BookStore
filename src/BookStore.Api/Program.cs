using BookStore.Api.Endpoints;
using BookStore.Api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Get Connection string frrom appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Register DbContext with SQLite provider to Dependency injection
builder.Services.AddDbContext<BookStoreDbContext>(options =>
    options.UseSqlite(connectionString));

var app = builder.Build();

app.MapBookEndpoints();
app.MapOrderEndpoints();

app.UseHttpsRedirection();

app.Run();