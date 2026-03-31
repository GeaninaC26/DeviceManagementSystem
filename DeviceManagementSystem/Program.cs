using Microsoft.Data.SqlClient;
using DeviceManagementSystem.Application;
using DeviceManagementSystem.Infrastructure;


var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.AddControllers();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
try
{
    using (var connection = new SqlConnection(connectionString))
    {
        connection.Open();
        Console.WriteLine("✓ Database connection successful!");
        connection.Close();
    }
}
catch (Exception ex)
{
    Console.WriteLine($"✗ Database connection failed: {ex.Message}");
}

if (app.Environment.IsDevelopment())
{
}

app.UseHttpsRedirection();


app.Run();

