namespace MyFirstDotnetApi.Models;

public record User(
    string UserId,
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    DateTime CreatedAt
);
