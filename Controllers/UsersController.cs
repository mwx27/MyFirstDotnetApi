using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFirstDotnetApi.Models;

namespace MyFirstDotnetApi.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]

public class UsersController(List<User> users) : ControllerBase
{

    private readonly List<User> _users = users;

    [HttpGet]
    public IActionResult GetAllUsers()
    {
        return Ok(_users);
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

        _users.Add(user);
        return Created($"/users/{user.UserId}", user);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(string id)
    {
        var user = _users.FirstOrDefault(u => u.UserId == id);
        return user is null ? NotFound(new { message = "User not found" }) : Ok(user);
    }

    [HttpPut("{id}")]
    public IActionResult Update(string id, UserRequest updatedData)
    {
        var user = _users.FirstOrDefault(u => u.UserId == id);
        if (user is null) return NotFound(new { message = "User not found" });

        var updatedUser = user with
        {
            FirstName = updatedData.FirstName,
            LastName = updatedData.LastName,
            Email = updatedData.Email,
            PhoneNumber = updatedData.PhoneNumber
        };

        _users.Remove(user);
        _users.Add(updatedUser);

        return Ok(updatedUser);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        var user = _users.FirstOrDefault(u => u.UserId == id);
        if (user is null) return NotFound(new { message = "User not found" });

        _users.Remove(user);
        return NoContent();
    }


}
