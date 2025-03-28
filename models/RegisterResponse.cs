using System.ComponentModel.DataAnnotations;

namespace MyFirstDotnetApi.Models;

/// <summary>
/// Response returned when a user successfully registers
/// </summary>
public record RegisterResponse
{
    /// <summary>
    /// Unique identifier for the newly created user
    /// </summary>
    /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
    [Required]
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Confirmation message
    /// </summary>
    /// <example>User registered successfully</example>
    public string Message { get; set; } = string.Empty;
}
