using ConsumerApp.Events;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDaprClient();  // This adds the Dapr client to access pub/sub

builder.Services.AddRazorPages();
builder.Services.AddSignalR();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
     {
         options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
     })
    .AddDapr(); // This line ads Dapr integration
builder.Services.AddEndpointsApiExplorer();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Ensure Dapr middleware is added for pub/sub
app.UseCloudEvents();  // This enables Dapr's event consumption
app.MapSubscribeHandler(); // This enables subscription to Dapr pubsub events
app.MapControllers(); 

app.UseAuthorization();

app.MapRazorPages();
app.MapHub<NotificationHub>("/notificationHub");
app.Run();
