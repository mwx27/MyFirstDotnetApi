using System.ComponentModel.DataAnnotations;

namespace MyFirstDotnetApi.Models;

/// <summary>
/// Request model for user registration
/// </summary>
public record RegisterRequest
{
    /// <summary>
    /// First name of the user
    /// </summary>
    /// <example>John</example>
    [Required]
    [StringLength(50)]
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Last name of the user
    /// </summary>
    /// <example>Doe</example>
    [Required]
    [StringLength(50)]
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Email address of the user (must be unique)
    /// </summary>
    /// <example>john.doe@example.com</example>
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Contact phone number of the user
    /// </summary>
    /// <example>+1234567890</example>
    [Phone]
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// Password for the user account
    /// </summary>
    /// <example>StrongP@ss123</example>
    [Required]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
    public string Password { get; set; } = string.Empty;
}
