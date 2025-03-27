using MyFirstDotnetApi.Models;
using MyFirstDotnetApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddSingleton<List<RegisteredUser>>();
builder.Services.AddSingleton<List<User>>();

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

app.Run();
