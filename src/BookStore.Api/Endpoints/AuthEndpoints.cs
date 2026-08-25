using BookStore.Api.Data;
using BookStore.Api.Interface;
using BookStore.Shared.Dtos;

namespace BookStore.Api;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/auth");

        // POST api/auth/register/UserDto
        group.MapPost("/register", async(UserDto dto, IAuthService authService, BookStoreDbContext context) =>
        {
             var user = await authService.RegisterAsync(dto);
             if(user is null)
            {
                return Results.BadRequest("This username is already exists.");
            }

            return Results.Ok(user);
        }).AllowAnonymous();

        // POST api/auth/login/UserDto
        group.MapPost("/login", async(UserDto dto, IAuthService authService, BookStoreDbContext context) =>
        {
            var token = await authService.LoginAsync(dto);
            if(token is null)
            {
                return Results.BadRequest("Invalid username or password.");
            }

            return Results.Ok(token);
        }).AllowAnonymous();   
    } 
}
