using System;
using BookStore.Shared.Dtos;

namespace BookStore.Api.Interface;

public interface IAuthService
{
    Task<User?> RegisterAsync(UserDto request);
    Task<string?> LoginAsync(UserDto request);
}
