using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFirstDotnetApi.Models;
using MyFirstDotnetApi.Helpers;

namespace MyFirstDotnetApi.Controllers;

/// <summary>
/// Handles user authentication operations including registration and login
/// </summary>
[ApiController]
[Route("auth")]
[Produces("application/json")]
[Tags("Authentication")]
public class AuthController(List<RegisteredUser> registeredUsers, List<User> users, IConfiguration config) : ControllerBase
{
    private readonly List<RegisteredUser> _registeredUsers = registeredUsers;
    private readonly List<User> _users = users;
    private readonly IConfiguration _config = config;

    /// <summary>
    /// Registers a new user in the system
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// 
    ///     POST /auth/register
    ///     {
    ///        "firstName": "John",
    ///        "lastName": "Doe",
    ///        "email": "john.doe@example.com",
    ///        "phoneNumber": "+1234567890",
    ///        "password": "strongPassword123"
    ///     }
    /// </remarks>
    /// <param name="request">New user registration details</param>
    /// <returns>Confirmation of registration with the user ID</returns>
    /// <response code="201">Returns the newly created user ID</response>
    /// <response code="400">If a user with the same email already exists</response>
    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(RegisterResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Register([FromBody] RegisterRequest request)
    {
        var exists = _registeredUsers.Any(u => u.Email == request.Email);
        if (exists) return BadRequest(new { message = "User with this email already exists" });

        var hashedPassword = PasswordHelper.HashPassword(request.Password);

        var uId = Guid.NewGuid().ToString();
        var createdAt = DateTime.UtcNow;

        var newUser = new RegisteredUser(
            userId: uId,
            firstName: request.FirstName,
            lastName: request.LastName,
            email: request.Email,
            phoneNumber: request.PhoneNumber,
            passwordHash: hashedPassword,
            createdAt: createdAt
        );

        _registeredUsers.Add(newUser);

        var user = new User(
            userId: uId,
            firstName: request.FirstName,
            lastName: request.LastName,
            email: request.Email,
            phoneNumber: request.PhoneNumber,
            createdAt: createdAt
        );

        _users.Add(user);

        var response = new RegisterResponse
        {
            UserId = newUser.UserId,
            Message = "User registered successfully"
        };

        return Created("/auth/register", response);
    }

    /// <summary>
    /// Authenticates a user and returns a JWT token
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// 
    ///     POST /auth/login
    ///     {
    ///        "email": "user@example.com",
    ///        "password": "password123"
    ///     }
    /// 
    /// The JWT token can be used to authorize further requests by adding it to the Authorization header:
    /// `Authorization: Bearer {token}`
    /// </remarks>
    /// <param name="request">Login credentials</param>
    /// <returns>JWT token and user ID</returns>
    /// <response code="200">Returns the JWT token</response>
    /// <response code="401">If credentials are invalid</response>
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var user = _registeredUsers.FirstOrDefault(u =>
      u.Email == request.Email &&
      u.PasswordHash == PasswordHelper.HashPassword(request.Password)
  );

        if (user is null) return Unauthorized();

        var token = JwtHelper.GenerateToken(user.UserId, _config);

        return Ok(new LoginResponse
        {
            Token = token,
            UserId = user.UserId,
            ExpiresAt = DateTime.UtcNow.AddMinutes(int.Parse(_config["Jwt:ExpiresInMinutes"]!))
        });
    }
}
