namespace MyFirstDotnetApi.Models;

public record RegisteredUser(
  string UserId,
  string FirstName,
  string LastName,
  string Email,
  string PhoneNumber,
  string PasswordHash,
  DateTime CreatedAt
);
