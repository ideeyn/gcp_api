var builder = WebApplication.CreateBuilder(args);

// Get the PORT environment variable or default to 8080
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Map the controllers
app.MapControllers();

// Start the application and listen on the specified port
app.Urls.Add($"http://0.0.0.0:{port}");
app.Run();
