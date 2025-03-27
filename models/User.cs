namespace MyFirstDotnetApi.Models;

/// <summary>
/// Represents a user in the system (public profile without credentials)
/// </summary>
public record User
{
    /// <summary>
    /// Unique identifier for the user
    /// </summary>
    public string UserId { get; init; } = string.Empty;

    /// <summary>
    /// User's first name
    /// </summary>
    public string FirstName { get; init; } = string.Empty;

    /// <summary>
    /// User's last name
    /// </summary>
    public string LastName { get; init; } = string.Empty;

    /// <summary>
    /// User's email address
    /// </summary>
    public string Email { get; init; } = string.Empty;

    /// <summary>
    /// User's contact phone number
    /// </summary>
    public string PhoneNumber { get; init; } = string.Empty;

    /// <summary>
    /// When the user account was created (UTC)
    /// </summary>
    public DateTime CreatedAt { get; init; }

    public User(string userId, string firstName, string lastName, string email, string phoneNumber, DateTime createdAt)
    {
        UserId = userId;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber;
        CreatedAt = createdAt;
    }
}
