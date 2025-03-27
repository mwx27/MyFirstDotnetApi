using System.ComponentModel.DataAnnotations;

namespace MyFirstDotnetApi.Models;

/// <summary>
/// Request model for creating or updating a user
/// </summary>
public record UserRequest
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
    /// Email address of the user
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
}
