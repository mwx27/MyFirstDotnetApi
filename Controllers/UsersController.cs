using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFirstDotnetApi.Models;

namespace MyFirstDotnetApi.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]

public class UsersController : ControllerBase
{
    private static readonly List<User> users = new();

    [HttpGet]
    public IActionResult GetAllUsers()
    {
        return Ok(users);
    }

    [HttpPost]
    [AllowAnonymous]
    public IActionResult CreateUser(UserRequest request)
    {
        var user = new User(
            UserId: Guid.NewGuid().ToString(),
            FirstName: request.FirstName,
            LastName: request.LastName,
            Email: request.Email,
            PhoneNumber: request.PhoneNumber,
            CreatedAt: DateTime.UtcNow
        );

        users.Add(user);
        return Created($"/users/{user.UserId}", user);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(string id)
    {
        var user = users.FirstOrDefault(u => u.UserId == id);
        return user is null ? NotFound(new { message = "User not found" }) : Ok(user);
    }

    [HttpPut("{id}")]
    public IActionResult Update(string id, UserRequest updatedData)
    {
        var user = users.FirstOrDefault(u => u.UserId == id);
        if (user is null) return NotFound(new { message = "User not found" });

        var updatedUser = user with
        {
            FirstName = updatedData.FirstName,
            LastName = updatedData.LastName,
            Email = updatedData.Email,
            PhoneNumber = updatedData.PhoneNumber
        };

        users.Remove(user);
        users.Add(updatedUser);

        return Ok(updatedUser);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        var user = users.FirstOrDefault(u => u.UserId == id);
        if (user is null) return NotFound(new { message = "User not found" });

        users.Remove(user);
        return NoContent();
    }


}
