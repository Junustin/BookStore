using BookStore.Api.Endpoints;
using BookStore.Api.Data;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Register OpenAPI
builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", policy =>
    {
        policy.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

// Get Connection string frrom appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Register DbContext with SQLite provider to Dependency injection
builder.Services.AddDbContext<BookStoreDbContext>(options =>
    options.UseSqlite(connectionString));

// Add input validation service
builder.Services.AddValidation();

var app = builder.Build();

app.UseCors("AllowBlazorClient");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapBookEndpoints();
app.MapOrderEndpoints();

// app.UseHttpsRedirection();

app.Run();