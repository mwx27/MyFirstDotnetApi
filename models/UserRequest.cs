namespace MyFirstDotnetApi.Models;

public record UserRequest(
  string FirstName,
  string LastName,
  string Email,
  string PhoneNumber
);
