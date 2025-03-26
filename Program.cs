using MyFirstDotnetApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var users = new List<User>();
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

app.Run();
