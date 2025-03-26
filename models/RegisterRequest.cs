namespace MyFirstDotnetApi.Models;

public record RegisterRequest(
  string FirstName,
  string LastName,
  string Email,
  string PhoneNumber,
  string Password
);
