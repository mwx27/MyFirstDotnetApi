using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFirstDotnetApi.Models;
using MyFirstDotnetApi.Helpers;

namespace MyFirstDotnetApi.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(List<RegisteredUser> registeredUsers, IConfiguration config) : ControllerBase
{

    private readonly List<RegisteredUser> _registeredUsers = registeredUsers;
    private readonly IConfiguration _config = config;

    [HttpPost("register")]
    [AllowAnonymous]
    public IActionResult Register([FromBody] RegisterRequest request)
    {
        var exists = _registeredUsers.Any(u => u.Email == request.Email);
        if (exists) return BadRequest(new { message = "User with this email already exists" });

        var hashedPassword = PasswordHelper.HashPassword(request.Password);

        var newUser = new RegisteredUser(
            UserId: Guid.NewGuid().ToString(),
            FirstName: request.FirstName,
            LastName: request.LastName,
            Email: request.Email,
            PhoneNumber: request.PhoneNumber,
            PasswordHash: hashedPassword,
            CreatedAt: DateTime.UtcNow
        );

        _registeredUsers.Add(newUser);

        return Created("/auth/register", new { newUser.UserId, message = "User registered successfully" });
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var user = _registeredUsers.FirstOrDefault(u =>
      u.Email == request.Email &&
      u.PasswordHash == PasswordHelper.HashPassword(request.Password)
  );

        if (user is null) return Unauthorized();

        var token = JwtHelper.GenerateToken(user.UserId, _config);

        return Ok(new
        {
            token,
            userId = user.UserId,
            expiresAt = DateTime.UtcNow.AddMinutes(int.Parse(_config["Jwt:ExpiresInMinutes"]!))
        });
    }
}
