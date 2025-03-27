using MyFirstDotnetApi.Models;
using MyFirstDotnetApi.Helpers;
using MyFirstDotnetApi.Extensions;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddControllers();

var users = new List<User>();
var registeredUsers = new List<RegisteredUser>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/ping", () => Results.Ok("API is running")).WithName("Ping");

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



