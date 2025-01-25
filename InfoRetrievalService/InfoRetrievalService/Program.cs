var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDaprClient();  // Add Dapr client services

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCloudEvents();  // Enable CloudEvents middleware for Dapr

app.MapControllers();

app.Run();
