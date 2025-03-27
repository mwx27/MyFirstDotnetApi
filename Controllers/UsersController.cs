using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFirstDotnetApi.Models;
using System.Security.Claims;

namespace MyFirstDotnetApi.Controllers;

/// <summary>
/// Manages user resources including CRUD operations
/// </summary>
[ApiController]
[Route("[controller]")]
[Authorize]
[Produces("application/json")]
[Tags("Users")]
public class UsersController(List<User> users) : ControllerBase
{
    private readonly List<User> _users = users;

    /// <summary>
    /// Retrieves all users in the system
    /// </summary>
    /// <returns>A list of all users</returns>
    /// <response code="200">Returns the list of users</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<User>), StatusCodes.Status200OK)]
    public IActionResult GetAllUsers()
    {
        return Ok(_users);
    }

    /// <summary>
    /// Creates a new user
    /// </summary>
    /// <param name="request">User data</param>
    /// <returns>The newly created user</returns>
    /// <response code="201">Returns the newly created user</response>
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(User), StatusCodes.Status201Created)]
    public IActionResult CreateUser(UserRequest request)
    {
        var user = new User(
            userId: Guid.NewGuid().ToString(),
            firstName: request.FirstName,
            lastName: request.LastName,
            email: request.Email,
            phoneNumber: request.PhoneNumber,
            createdAt: DateTime.UtcNow
        );

        _users.Add(user);
        return Created($"/users/{user.UserId}", user);
    }

    /// <summary>
    /// Retrieves a specific user by ID
    /// </summary>
    /// <param name="id">User ID</param>
    /// <returns>The requested user</returns>
    /// <response code="200">Returns the user</response>
    /// <response code="404">If the user is not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(string id)
    {
        var user = _users.FirstOrDefault(u => u.UserId == id);
        return user is null ? NotFound(new { message = "User not found" }) : Ok(user);
    }

    /// <summary>
    /// Updates an existing user
    /// </summary>
    /// <param name="id">User ID</param>
    /// <param name="updatedData">Updated user data</param>
    /// <returns>The updated user</returns>
    /// <response code="200">Returns the updated user</response>
    /// <response code="404">If the user is not found</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(string id, UserRequest updatedData)
    {
        var user = _users.FirstOrDefault(u => u.UserId == id);
        if (user is null) return NotFound(new { message = "User not found" });

        var updatedUser = new User(
            userId: user.UserId,
            firstName: updatedData.FirstName,
            lastName: updatedData.LastName,
            email: updatedData.Email,
            phoneNumber: updatedData.PhoneNumber,
            createdAt: user.CreatedAt
        );

        _users.Remove(user);
        _users.Add(updatedUser);

        return Ok(updatedUser);
    }

    /// <summary>
    /// Deletes a user
    /// </summary>
    /// <param name="id">User ID</param>
    /// <returns>No content</returns>
    /// <response code="204">If the user was successfully deleted</response>
    /// <response code="404">If the user is not found</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(string id)
    {
        var user = _users.FirstOrDefault(u => u.UserId == id);
        if (user is null) return NotFound(new { message = "User not found" });

        _users.Remove(user);
        return NoContent();
    }

    /// <summary>
    /// Retrieves the profile of the currently authenticated user
    /// </summary>
    /// <returns>The current user's profile</returns>
    /// <response code="200">Returns the current user</response>
    /// <response code="401">If the authentication token is invalid</response>
    /// <response code="404">If the user is not found</response>
    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetCurrentUser()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (claim is null) return Unauthorized(new { message = "Invalid or missing token." });

        var userId = claim.Value;
        var user = _users.FirstOrDefault(u => u.UserId == userId);

        if (user is null) return NotFound(new { message = "User not found" });

        return Ok(user);
    }
}
