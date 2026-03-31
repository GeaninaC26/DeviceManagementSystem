using Microsoft.Data.SqlClient;
using DeviceManagementSystem.Application;
using DeviceManagementSystem.Infrastructure;
using Microsoft.OpenApi;


var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.AddControllers();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "Device Management System API",
            Description = "API for managing devices and user-device assignments"
        });
    }
);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();



app.UseCors("AllowAll");
app.MapControllers();
// app.UseHttpsRedirection();

if (builder.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    });
}

app.Run();

