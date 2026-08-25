using System;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Shared.Dtos;

public record UserDto(
    [Required(ErrorMessage = "Username is required")]
    string  UserName,
    [Required(ErrorMessage = "Password is required")]
    string Password
);

public record AuthResultDto(
    bool Success,
    string? Token = null,
    string? UserName = null,
    string? Role = null,
    string? ErrorMessage = null
);
