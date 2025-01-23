var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDaprClient();  // This adds the Dapr client to access pub/sub

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers().AddDapr(); // This line ads Dapr integration
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Ensure Dapr middleware is added for pub/sub
app.UseCloudEvents();  // This enables Dapr's event consumption

app.MapSubscribeHandler(); // This enables subscription to Dapr pubsub events

app.UseAuthorization();

app.MapRazorPages();

app.Run();
