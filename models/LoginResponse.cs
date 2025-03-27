using System.ComponentModel.DataAnnotations;

namespace MyFirstDotnetApi.Models;

/// <summary>
/// Response returned when a user successfully logs in
/// </summary>
public record LoginResponse
{
    /// <summary>
    /// JWT token for authentication
    /// </summary>
    /// <example>eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...</example>
    [Required]
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// Unique identifier of the logged-in user
    /// </summary>
    /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
    [Required]
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// UTC timestamp when the token will expire
    /// </summary>
    /// <example>2023-12-31T23:59:59Z</example>
    [Required]
    public DateTime ExpiresAt { get; set; }
}
