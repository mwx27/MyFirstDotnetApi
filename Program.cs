using MyFirstDotnetApi.Models;
using MyFirstDotnetApi.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var users = new List<User>();
var registeredUsers = new List<RegisteredUser>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/ping", () => Results.Ok("API is running")).WithName("Ping");

app.MapGet("/users", () => Results.Ok(users)).WithName("GetUsers");

app.MapPost("/users", (UserRequest request) =>
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
    return Results.Created($"/users/{user.UserId}", user);
})
.WithName("CreateUser");

app.MapGet("/users/{id}", (string id) =>
{
    var user = users.FirstOrDefault(u => u.UserId == id);

    if (user is null) return Results.NotFound(new { message = "User not found" });

    return Results.Ok(user);
})
.WithName("GetUserById");

app.MapPut("/users/{id}", (string id, UserRequest updatedData) =>
{
    var user = users.FirstOrDefault(u => u.UserId == id);

    if (user is null) return Results.NotFound(new { message = "User not found" });

    var updatedUser = user with
    {
        FirstName = updatedData.FirstName,
        LastName = updatedData.LastName,
        Email = updatedData.Email,
        PhoneNumber = updatedData.PhoneNumber
    };

    users.Remove(user);
    users.Add(updatedUser);

    return Results.Ok(updatedUser);
})
.WithName("UpdatedUser");

app.MapDelete("/users/{id}", (string id) =>
{
    var user = users.FirstOrDefault(u => u.UserId == id);

    if (user is null) return Results.NotFound(new { message = "User not found" });

    users.Remove(user);
    return Results.NoContent();
})
.WithName("DeleteUser");

app.MapPost("/auth/register", (RegisterRequest request) =>
{
    var exists = registeredUsers.Any(u => u.Email == request.Email);
    if (exists) return Results.BadRequest(new { message = "User with this email already exists" });

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

    registeredUsers.Add(newUser);

    return Results.Created("/auth/register", new { newUser.UserId, message = "User registered successfully" });
});

app.MapPost("/auth/login", (LoginRequest request, IConfiguration config) =>
{
    var user = registeredUsers.FirstOrDefault(u =>
        u.Email == request.Email &&
        u.PasswordHash == PasswordHelper.HashPassword(request.Password)
    );

    if (user is null) return Results.Unauthorized();

    var token = JwtHelper.GenerateToken(user.UserId, config);

    return Results.Ok(new
    {
        token,
        userId = user.UserId,
        expiresAt = DateTime.UtcNow.AddMinutes(int.Parse(config["Jwt:ExpiresInMinutes"]!))
    });
});

app.Run();
